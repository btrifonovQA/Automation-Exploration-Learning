using MovieCatalog.DTOs;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace MovieCatalog
{
    [TestFixture]
    public class Tests
    {
        private RestClient client;
        private static string movieId;
        private readonly string nonExistingMovieId = "TheseAreNotTheIdsYouAreLookingFor";

        [OneTimeSetUp]
        public void Setup()
        {
            string jwtToken = GetJwtToken("exambi@softuni.com", "swordfish123");
            RestClientOptions options = new RestClientOptions("http://144.91.123.158:5000")
            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };
            client = new RestClient(options);
        }

        private string GetJwtToken(string email, string password)
        {
            RestClient client = new RestClient("http://144.91.123.158:5000");
            RestRequest request = new RestRequest("/api/User/Authentication", Method.Post);
            request.AddJsonBody(new { email, password });
            RestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var token = content.GetProperty("accessToken").GetString();

                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException("Token not found in the response.");
                }
                return token;
            }
            else
            {
                throw new InvalidOperationException($"Failed to authenticate. Status code: {response.StatusCode}, Response: {response.Content}");
            }
        }

        [Order(1)]
        [Test]
        public void CreateNewMovie_WithRequiredFields_ShouldSuccess()
        {
            MovieDTO movie = new MovieDTO
            {
                Title = "QA 2 - Electric boogaloo",
                Description = "The best tests the world is yet to see."
            };

            RestRequest request = new RestRequest("/api/Movie/Create", Method.Post);
            request.AddJsonBody(movie);
            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(readyResponse.Movie, Is.Not.Null);
                Assert.That(readyResponse.Movie.Id, Is.Not.Null.And.Not.Empty);
                Assert.That(readyResponse.Msg, Is.EqualTo("Movie created successfully!"));
            }
            movieId = readyResponse.Movie.Id;
        }

        [Order(2)]
        [Test]
        public void EditMovieRequest_ShouldEditMovieDescriptionAndTitle()
        {
            RestRequest request = new RestRequest("/api/Movie/Edit", Method.Put);

            request.AddQueryParameter("movieId", movieId);
            request.AddJsonBody(new
            {
                Title = "Updated Title",
                Description = "Changed description"
            });

            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse.Msg, Is.EqualTo("Movie edited successfully!"));
        }

        [Order(3)]
        [Test]
        public void GetAllMovies_ShouldReturnNonEmptyArray()
        {
            RestRequest request = new RestRequest("/api/Catalog/All", Method.Get);
            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            List<MovieDTO> readyResponse = JsonSerializer.Deserialize<List<MovieDTO>>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse, Is.Not.Empty);
        }

        [Order(4)]
        [Test]
        public void DeleteExistingMovie_ShouldSucceed()
        {
            RestRequest request = new RestRequest("/api/Movie/Delete", Method.Delete);
            request.AddQueryParameter("movieId", movieId);

            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse.Msg, Is.EqualTo("Movie deleted successfully!"));
        }

        [Order(5)]
        [Test]
        public void CreateMovie_WithoutRequiredFields_ShouldReturnBadRequest()
        {
            MovieDTO movie = new MovieDTO
            {
                Title = "",
                Description = ""
            };

            RestRequest request = new RestRequest("/api/Movie/Create", Method.Post);
            request.AddJsonBody(movie);

            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Order(6)]
        [Test]
        public void EditNonExistingMovie_ShouldReturnBadRequest()
        {
            RestRequest request = new RestRequest("/api/Movie/Edit", Method.Put);
            request.AddQueryParameter("movieId", nonExistingMovieId);

            request.AddJsonBody(new
            {
                Title = "R2",
                Description = "D2"
            });

            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse.Msg, Is.EqualTo("Unable to edit the movie! Check the movieId parameter or user verification!"));
        }

        [Order(7)]
        [Test]
        public void DeleteNonExistingMovie_ShouldReturnBadRequest()
        {
            RestRequest request = new RestRequest("/api/Movie/Delete", Method.Delete);
            request.AddQueryParameter("movieId", nonExistingMovieId);

            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse.Msg, Is.EqualTo("Unable to delete the movie! Check the movieId parameter or user verification!"));

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.client?.Dispose();
        }
    }
}