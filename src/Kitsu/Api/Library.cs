using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kitsu.Api
{
    public class Library
    {
        public static async Task<dynamic> GetAnime(int userId, int animeId)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/users/{userId}/library-entries?filter[animeId]={animeId}");
            return JsonConvert.DeserializeObject(json);
        }

        public static async Task<string> AddAnime(int userId, int animeId, Status status)
        {
            string body = @"{  
               ""data"":{  
                  ""type"":""libraryEntries"",
                  ""attributes"":{ ""status"":""STATUS"" },
                  ""relationships"":{  
                     ""user"":{  
                        ""data"":{  
                           ""type"":""users"",
                           ""id"":""USERID""
                        }
                     },
                     ""anime"":{  
                        ""data"":{  
                           ""type"":""anime"",
                           ""id"":""ANIMEID""
                        }
                     }
                  }
               }
            }";

            body = body.Replace("USERID", userId.ToString())
                       .Replace("ANIMEID", animeId.ToString())
                       .Replace("STATUS", status.ToString());

            string responseData = string.Empty;
            using (var content = new StringContent(body, System.Text.Encoding.Default, "application/vnd.api+json"))
            {
                using (var response = await Kitsu.Client.PostAsync($"{Kitsu.BaseUri}/library-entries", content))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }

            return responseData;
        }
    }
}
