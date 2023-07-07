using CurriculumSharingPortal.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurriculumSharingPortal.Desktop.Model
{
	public class CurriculumSharingPortalApiService : IDisposable
    {
        private readonly HttpClient _client;

        public CurriculumSharingPortalApiService(string baseAdress)
        {
              _client = new HttpClient() 
              { 
                  BaseAddress = new Uri(baseAdress)
              };
        }

		public void Dispose()
		{
            _client.Dispose();
		}

		#region Authentication

		public async Task<bool> LoginAsync(string userName, string password)
        {
            LoginDto user = new LoginDto
            {
                UserName = userName,
                Password = password
            };

            var response = await _client.PostAsJsonAsync("api/account/login", user);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task LogoutAsync()
        {
            var response = await _client.PostAsync("api/account/logout", null);

            if(response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        #endregion

        #region Subject

        public async Task<IEnumerable<SubjectDto>> LoadSubjectsAsync()
        {
            var response = await _client.GetAsync("api/subjects");

            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<SubjectDto>>();
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task CreateSubjectAsync(SubjectDto subject)
        {
            var response = await _client.PostAsJsonAsync("api/subjects", subject);

            subject.Id = (await response.Content.ReadAsAsync<SubjectDto>()).Id;

            if(!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned: " + response.StatusCode);
            }
        }

        public async Task UpdateSubjectAsync(SubjectDto subject)
        {
            var response = await _client.PutAsJsonAsync($"api/subjects/{subject.Id}", subject);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned: " + response.StatusCode);
            }
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/subjects/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned: " + response.StatusCode);
            }
        }

        #endregion

        #region Curriculum

        public async Task<IEnumerable<CurriculumDto>> LoadCurriculumsAsync(int subjectId)
        {
            var response = await _client.GetAsync($"api/curriculums/{subjectId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<CurriculumDto>>();
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task<IEnumerable<CurriculumDto>> LoadCurriculumsAsync(string name)
        {
            var response = await _client.GetAsync($"api/curriculums/name/{name}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<CurriculumDto>>();
            }

            throw new NetworkException("Service returned: " + response.StatusCode);
        }

        public async Task UpdateCurriculumAsync(CurriculumDto curriculum)
        {
            var response = await _client.PutAsJsonAsync($"api/curriculums/{curriculum.Id}", curriculum);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        public async Task DeleteCurriculumAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/curriculums/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        public async Task DeleteReviewsAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/curriculums/reviews/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

		#endregion
	}
}
