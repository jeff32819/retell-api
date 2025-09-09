
using System.Collections.Generic;

namespace RetellApi.Models
{
    public class CreateChatResponse
    {
        public string chat_id { get; set; }
        public string chat_type { get; set; }
        public string agent_id { get; set; }
        public int agent_version { get; set; }
        public string agent_name { get; set; }
        public string chat_status { get; set; }
        public long start_timestamp { get; set; }
        public string transcript { get; set; }
        public List<object> message_with_tool_calls { get; set; }
        public ChatCost chat_cost { get; set; }
        public bool opt_out_sensitive_data_storage { get; set; }
        public string data_storage_setting { get; set; }

        public class ChatCost
        {
            public List<object> product_costs { get; set; }
            public int combined_cost { get; set; }
        }
    }
}

