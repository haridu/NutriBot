using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

using Nutribot.models;

namespace Weather_Bot
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<Timeline> timelineTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://hariapp.azurewebsites.net");
            this.timelineTable = this.client.GetTable<Timeline>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task AddTimeline(Timeline timeline)
        {
            await this.timelineTable.InsertAsync(timeline);
        }

        public async Task<List<Timeline>> GetTimelines()
        {
            return await this.timelineTable.ToListAsync();
        }

        public async Task UpdateTimeline(Timeline timeline)
        {
            await this.timelineTable.UpdateAsync(timeline);
        }

        public async Task DeleteTimeline(Timeline timeline)
        {
            await this.timelineTable.DeleteAsync(timeline);
        }
    }
}