using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verdandi;

namespace skuld
{
    partial class server_worker : worker
    {
        private packet togglegm(packet p, object session)
        {
            string name = p.String;            
            
                int idx = data.accounts.FindIndex(x => x.name == name);
                if (idx != -1)
                {
                    if (data.accounts[idx].accesslevel == (int)privilege.gm)
                    {
                        data.accounts[idx].accesslevel = (int)privilege.user;
                        return new packet(p,status_message("changed to user"));
                    }
                    if (data.accounts[idx].accesslevel == (int)privilege.user)
                    {
                        data.accounts[idx].accesslevel = (int)privilege.gm;
                        return new packet(p,status_message("changed to GM"));
                    }
                    return new packet(p, status_message("nothing happened"));
                }
                else return new packet(p, status_message("not exists"));
           
        }
        private packet do_auth(packet p, object session)
        {
            session session_current = (session)session;

            short authstage = p.ReadSS();

            short authphase_fail = 0;
            short authphase_credentials = 1;
            short authphase_proof = 2;
            short authphase_done = 3;

            //if for some reason server got unexpected code immideately return
            if (authstage <= authphase_fail||authstage>authphase_proof)
                return new packet(p, authphase_fail, status_message("auth fail unrelated"));

            //first phase of auth
            if (authstage == authphase_credentials)//client provide credentials
            {              

                long pass_a = p.ReadSL();
                long pass_b = p.ReadSL();

                //string pub = data.UniqueID.decrypt_auth_info(p.String8);
                string pub = p.String;

                //if (pub != session_current.remote.remote_uid)
                 //   log.Add("client misplace its id");

                log.Add(session_current.connection_uid.ToString()+" start auth");

                account a = null;
                ///every email+pub pair unique
                ///redo later possible bug
               
                //-----------------------
                int idx = data.accounts.FindIndex(x =>x.rsa_pub_identity == pub);//
                //--------------

                if (idx == -1)//nothing found
                {
                    log.Add(session_current.connection_uid.ToString() + " no such account to auth, creating new");

                    a = new account(pub, pass_a, pass_b, data.account_uid_provider++);
                    //
                    idx = data.accounts.Count;
                    //
                    data.accounts.Add(a);
                    //continuiuing                    
                }

                a = data.accounts[idx];

                //user need to set pass considered to use only once when setting things up
                //REDO later;
                if (a.pass_a == 0 && a.pass_b == 0)
                {
                    //setting to any pass user provide
                    a.pass_a = pass_a;a.pass_b = pass_b;
                }

                if (a.pass_a == pass_a && a.pass_b == pass_b)
                {
                    //okay. server recognises clients account now it needs check identity;
                    Random r = new Random((int)DateTime.UtcNow.Ticks);
                    int random = r.Next();
                    //
                   
                    //a.auth_identity.remote_public_rsa_info = a.rsa_pub_identity;

                    data.awaiting_proofs[random]=a;
                    //               
                    //string authstr = a.auth_identity.encrypt_auth_token(random);
                    //
                    //data.accounts[idx] = a;
                    //
                    return new packet(p, authphase_proof/*,session_current.connection_uid*/ //authstr
                        );
                }
                else return new packet(p, authphase_fail, status_message("wrong pass"));
           }
            //second phase of auth
            if (authstage == authphase_proof)
            {

                int authtoken = p.ReadSI();//client sends stage,token                

                log.Add(session_current.connection_uid.ToString()+" continue auth with '" + authtoken.ToString() + "' token");

                if (data.awaiting_proofs.ContainsKey(authtoken))
                {

                    account acc = data.awaiting_proofs[authtoken];
                    //
                    acc.lastactive = DateTime.UtcNow.Ticks;
                    session_current.session_account = acc;
                    //
                    data.awaiting_proofs.Remove(authtoken);
                    //
                    if (!data.logged_in.Exists(x => x == acc))
                        data.logged_in.Add(acc);
                    else { data.logged_in.Remove(acc); data.logged_in.Add(acc); }
                    //
                    log.Add(session_current.connection_uid.ToString() + " logged with " + acc.accountid.ToString() + " uid");
                    //
                    return new packet(p, authphase_done, status_message("ok"));
                }
            }
            return new packet(p, authphase_fail, status_message("wrong pass"));
        }
        private packet do_request_restore_auth(packet p, object session)
        {
            Random r = new Random((int)DateTime.UtcNow.Ticks);
            int random = r.Next();
            log.Add(random.ToString());
            //here send random code to email
            return null;
        }
        private packet do_confirm_restore_auth(packet p, object session)
        {
            //here read random code and new rsapub;
            return null;
        }
        private packet impersonate(packet p, object session)
        {
            session session_current = (session)session;

            byte name = 1;
            byte email = 2;
            byte companyname = 3;

            byte mode = p.ReadUB();

            string client_data = p.String;

          

            if (mode == name)
            {
                if(session_current.session_account.accesslevel>=3) return new packet(p, status_message("only user can renaimed"));

                session_current.session_account.name = client_data;
                return new packet(p, status_message("ok"));
            }
            if (mode == email)
            {
                session_current.session_account.email = client_data;
                return new packet(p, status_message("email registered"));
            }
            if (mode == companyname)
            {
                //session_current.session_account.
            }

            return null;
            //long passA = p.ReadSL();
            //long passB = p.ReadSL();
            //string name = p.String8;
            //string name2 = " '" + name + "' ";

            //log.Add("try to create new account: " + name2);
            //if (check_name_is_valid(name))
            //{
            //    if (!data.accounts.Exists(x => x.name == name))
            //    {
            //        account a = new account(name, data.accounts.Count);
            //        a.accesslevel = (int)privilege.user;
            //      //  a.passA = passA;
            //     //   a.passB = passB;

            //        data.accounts.Add(a);

            //        log.Add("new account :" + name2 + " created");
            //        return new packet(p.target, p.remote_command, "new account :" + name2 + " created");
            //    }
            //    else
            //    {
            //        log.Add(name2 + " already exists");
            //        return new packet(p.target, p.remote_command, name2 + " already exists");
            //    }
            //}
            //else
            //{
            //    if (name != "" && !data.accounts.Exists(x => x.name == "admin"))
            //    {
            //        account a = new account(name, data.accounts.Count);
            //        a.accesslevel = (int)privilege.admin;
            //     //   a.passA = passA;
            //      //  a.passB = passB;
            //        //session ss = server.sessions.Find(x=>x==this.accept_random
            //        //foreach(skuld.service.service in 
            //        //foreach (worker_id wi in a.worker_id) 
            //        //{

            //        //} 
            //        //a.worker_id.Add(new worker_id(
            //        data.accounts.Add(a);
            //        //this.DumpData();
            //        //data.Save();
            //        log.Add("THE ONE AND ONLY ADMIN ACCOUNT CREATED PASSWORD CANNOT BE CHANGED UNTIL RESETUP");
            //        return new packet(p.target, p.remote_command, "THE ONE AND ONLY ADMIN ACCOUNT CREATED PASSWORD CANNOT BE CHANGED UNTIL RESETUP");
            //    }
            //    log.Add("name: " + name2 + " is invalid no action performed");
            //    return new packet(p.target, p.remote_command, "name: " + name2 + " is invalid no action performed");
            //}
        }

    }
}
