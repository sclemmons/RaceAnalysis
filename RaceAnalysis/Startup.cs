using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RaceAnalysis.Models;

[assembly: OwinStartupAttribute(typeof(RaceAnalysis.Startup))]
namespace RaceAnalysis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            var context = new RaceAnalysisDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //create a Admin super user who will maintain the website                  
                var user = new ApplicationUser();
                user.UserName = "scott_clemmons@hotmail.com";
                user.Email = "scott_clemmons@hotmail.com";

                string userPWD = "@Tritone123";

                 var chkUser = UserManager.Create(user, userPWD);
             
                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            

          
        }
    }
}
