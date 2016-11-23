using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

using Nutribot.models;

namespace Weather_Bot
{
    public class AzureLoginManager
    {

        private static AzureLoginManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<login> loginTable;

        private AzureLoginManager()
        {
            this.client = new MobileServiceClient("http://hariapp.azurewebsites.net");
            this.loginTable = this.client.GetTable<login>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureLoginManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureLoginManager();
                }

                return instance;
            }
        }

        public async Task AddTimeline(login login)
        {
            await this.loginTable.InsertAsync(login);
        }

        public async Task<List<login>> Getlogins()
        {
            return await this.loginTable.ToListAsync();
        }

        public async Task Updatelogin(login login)
        {
            await this.loginTable.UpdateAsync(login);
        }

        public async Task Deletelogin(login login)
        {
            await this.loginTable.DeleteAsync(login);
        }
    }
}