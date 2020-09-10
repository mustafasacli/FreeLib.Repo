using Net.FreeLibrary.Core;

namespace Net.FreeLibrary.QueryBuilding
{
    public class Query : IQuery
    {
        private Hashmap h_prms = null;
        private string text_ = string.Empty;

        public Query()
        {
            h_prms = new Hashmap();
        }

        public Hashmap Parameters
        {
            get { return h_prms; }
            set { h_prms = value; }
        }

        public string Text
        {
            get { return text_; }
            set { text_ = value; }
        }
    }
}
