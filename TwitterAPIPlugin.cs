using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNWS
{
    class TwitterAPIPlugin : TwitterPlugin{
        
        Private List<User> GetUser(){

            using(var newContext = new TweetContext()){
                try{
                    List<User> users = newContext.Users.Where(b => true).Include(base => base.Following).ToList();
                    return users;
                }
                catch(Exception){
                    return null;
                }
            }
        }

        public override HTTPResponse GetResponse (HTTPRequest request){
            
            HTTPResponse response = new HTTPResponse(200);

            string username = request.getRequestByKey("user");
            string password = request.getRequestByKey("password");
            string following = request.getRequestByKey("following");
            string timeline = request.getRequestByKey("timline");
            string message = request.getRequestByKey("message");
            string []url = request.Filename.split("?");

            try{
                if(url[0] == "users"){
                    if(request.Method == "GET")
                    {//taught by 600611001
                        string js = JsonConvert.SerializeObject(GetUser());
                        response.body = Encoding.UTF8.GetBytes(js);
                    }
                    else if(request.Method == "POST")
                    {
                        if(username != null && password != null){
                            Twitter.AddUser(username, password);
                        }
                    }
                    else if(request.Method == "DELETE")
                    {
                        if(username != null && following != null)
                        {
                            Twitter account = new Twitter(username);
                            account.RemoveFollowing(following);
                        }
                    }
                }
                else if(url[0] == "tweets")
                {
                    if (username != null)
                    {
                        if(request.Method == "GET"){
                            //taught by 600611001
                            Twitter account = new Twitter(username);
	                            string js = JsonConvert.SerializeObject(account.GetUserTimeline());
	                            response.body = Encoding.UTF8.GetBytes(js);
	                            if (timeline != null)
	                            {
	                                string js_1 = JsonConvert.SerializeObject(account.GetFollowingTimeline());
	                                response.body = Encoding.UTF8.GetBytes(js_1);
	                            }
                        }
                        else if (request.Method == "POST")
	                        {
	                            Twitter account = new Twitter(user);
	                            account.PostTweet(message); 
	                        }
                    }
                }
            }
            catch (Exception except)
                //taught by 600611001
	            {
	                StringBuilder sb = new StringBuilder();
	                Console.WriteLine(except.ToString());
	                sb.Append(String.Format("Error [{0}], please go back to <a href=\"/twitter\">login page</a> to try again", ex.Message));
	                response.body = Encoding.UTF8.GetBytes(sb.ToString());
	                return response;
	            }
	            response.type = "application/json; charset=UTF-8";
	            return response;
        }

    }
    
}