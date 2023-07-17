using OnlineMessageManagement.Models;
using System.Data.SqlClient;

namespace OnlineMessageManagement.Services
{
    public class SocialServiceServices: ISocialServiceServices
    {
        public string  Constr { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public SocialServiceServices(IConfiguration configuration)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DefaultConnection");
        
        }

        public List<SocialService> GetSocialServiceRecord()
        {
            List<SocialService> socialServiceList = new List<SocialService>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("GetAllSocialServices", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader rdr=cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        SocialService ssr = new SocialService
                        {
                            ServiceId = Convert.ToInt32(rdr["ServiceId"]),
                            ServiceName = rdr["ServiceName"].ToString(),
                            ServiceStatus = Convert.ToInt32(rdr["ServiceStatus"])
                        };

                        socialServiceList.Add(ssr);

                    }
                    
                }
                return socialServiceList.ToList();
            }
                
            catch(Exception ) {
                throw;
            }

        }



    }

    public interface ISocialServiceServices
    {
        public List<SocialService> GetSocialServiceRecord();
    }
}
