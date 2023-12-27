using GalleryOfLuna.Vk.Configuration;
using GalleryOfLuna.Vk.Responses;
using GalleryOfLuna.Vk.Responses.Video;

using Microsoft.Extensions.Options;

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GalleryOfLuna.Vk
{
    public sealed class VkClient
    {
        private const string ApiVersion = "5.131";
        private const string ApiEndpoint = "https://api.vk.com/";

        private readonly HttpClient _httpClient;
        private readonly VkConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new();

        public VkClient(HttpClient httpClient, IOptions<VkConfiguration> configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration.Value;

            SetupJsonOptions();
            SetupHttpClient();
        }

        private void SetupHttpClient()
        {
            _httpClient.BaseAddress ??= new Uri(ApiEndpoint);

            _httpClient.DefaultRequestHeaders.Add("Content", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Luna's Gallery implementation of VK API SDK for .NET");
        }

        private void SetupJsonOptions()
        {
            _jsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }


        public async Task<Responses.Docs.GetUploadServerResponse> DocsGetUploadServerAsync(
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new QueryBuilder()
                .Add(QueryParameters.AccessToken, _configuration.AccessToken)
                .Add(QueryParameters.Version, ApiVersion);

            var requestUriBuilder = new UriBuilder(_httpClient.BaseAddress!);
            requestUriBuilder.Path = "/method/docs.getUploadServer";
            requestUriBuilder.Query = queryBuilder.ToString();

            return await SendRequestAsync<VkResponse<Responses.Docs.GetUploadServerResponse>>(
                requestUriBuilder.ToString(),
                HttpMethod.Get,
                cancellationToken);
        }

        public async Task<Responses.Photos.GetWallUploadServerResponse> PhotosGetWallUploadServerAsync(
            long groupId,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new QueryBuilder()
                .Add(QueryParameters.AccessToken, _configuration.AccessToken)
                .Add(QueryParameters.Version, ApiVersion)
                .Add(QueryParameters.GroupId, groupId);

            var requestUriBuilder = new UriBuilder(_httpClient.BaseAddress!);
            requestUriBuilder.Path = "/method/photos.getWallUploadServer";
            requestUriBuilder.Query = queryBuilder.ToString();

            return await SendRequestAsync<VkResponse<Responses.Photos.GetWallUploadServerResponse>>(
                requestUriBuilder.ToString(),
                HttpMethod.Get,
                cancellationToken);
        }

        public async Task<IEnumerable<Responses.Photos.PhotoResponse>> SaveWallPhotoAsync(
            long groupId,
            string photo,
            long server,
            string hash,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new QueryBuilder()
                .Add(QueryParameters.AccessToken, _configuration.AccessToken)
                .Add(QueryParameters.Version, ApiVersion)
                .Add(QueryParameters.GroupId, groupId)
                .Add(QueryParameters.Server, server)
                .Add(QueryParameters.Hash, hash)
                .Add(QueryParameters.Photo, photo);

            var requestUriBuilder = new UriBuilder(_httpClient.BaseAddress!);
            requestUriBuilder.Path = "/method/photos.saveWallPhoto";
            requestUriBuilder.Query = queryBuilder.ToString();

            var result = await SendRequestAsync<VkResponse<IEnumerable<Responses.Photos.PhotoResponse>>>(
                requestUriBuilder.ToString(),
                HttpMethod.Get,
                cancellationToken);

            return result.Response;
        }

        public async Task Post(
            long ownerId,
            string message,
            string attachments,
            string copyright,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new QueryBuilder()
                .Add(QueryParameters.AccessToken, _configuration.AccessToken)
                .Add(QueryParameters.Version, ApiVersion)
                .Add(QueryParameters.Message, message)
                .Add(QueryParameters.Copyright, copyright)
                .Add(QueryParameters.OwnerId, ownerId)
                .Add(QueryParameters.Attachments, attachments);

            var requestUriBuilder = new UriBuilder(_httpClient.BaseAddress!);
            requestUriBuilder.Path = "/method/wall.post";
            requestUriBuilder.Query = queryBuilder.ToString();

            // TODO: Add overload or implement response handling
            await SendRequestAsync<object>(
                requestUriBuilder.ToString(),
                HttpMethod.Get,
                cancellationToken);
        }

        public async Task<Responses.Docs.SaveResponse> DocsSave(
            string file,
            string title,
            string tags,
            bool returnTags,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new QueryBuilder()
                .Add(QueryParameters.AccessToken, _configuration.AccessToken)
                .Add(QueryParameters.Version, ApiVersion)
                .Add(QueryParameters.File, file)
                .Add(QueryParameters.Title, title)
                .Add(QueryParameters.Tags, tags)
                .Add(QueryParameters.ReturnTags, returnTags);

            var requestUriBuilder = new UriBuilder(_httpClient.BaseAddress!);
            requestUriBuilder.Path = "/method/docs.save";
            requestUriBuilder.Query = queryBuilder.ToString();

            return await SendRequestAsync<VkResponse<Responses.Docs.SaveResponse>>(
                requestUriBuilder.ToString(),
                HttpMethod.Get,
                cancellationToken);
        }

        public async Task DocsDelete(
            long ownerId,
            long docId,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new QueryBuilder()
                .Add(QueryParameters.AccessToken, _configuration.AccessToken)
                .Add(QueryParameters.Version, ApiVersion)
                .Add(QueryParameters.OwnerId, ownerId)
                .Add(QueryParameters.DocId, docId);

            var requestUriBuilder = new UriBuilder(_httpClient.BaseAddress!);
            requestUriBuilder.Path = "/method/docs.delete";
            requestUriBuilder.Query = queryBuilder.ToString();

            await SendRequestAsync<object>(
                requestUriBuilder.ToString(),
                HttpMethod.Get,
                cancellationToken);
        }

        public async Task<Responses.Video.SaveResponse> VideoSave(
            string name,
            string description,
            long groupId,
            bool noComments,
            bool repeat,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new QueryBuilder()
                .Add(QueryParameters.AccessToken, _configuration.AccessToken)
                .Add(QueryParameters.Version, ApiVersion)
                .Add(QueryParameters.Name, name)
                .Add(QueryParameters.Description, description)
                .Add(QueryParameters.GroupId, groupId)
                .Add(QueryParameters.NoComments, ApiVersion)
                .Add(QueryParameters.Repeat, repeat);

            var requestUriBuilder = new UriBuilder(_httpClient.BaseAddress!);
            requestUriBuilder.Path = "/method/video.save";
            requestUriBuilder.Query = queryBuilder.ToString();

            return await SendRequestAsync<VkResponse<Responses.Video.SaveResponse>>(
                requestUriBuilder.ToString(),
                HttpMethod.Get,
                cancellationToken);
        }

        public async Task<UploadVideoResponse> UploadVideoAsync(
            string uploadUrl,
            string videoFormat,
            byte[] fileContent,
            CancellationToken cancellationToken = default)
        {
            var content = GetUploadContent("video_file", videoFormat, fileContent);

            return await SendRequestAsync<UploadVideoResponse>(uploadUrl, HttpMethod.Post, content, cancellationToken);
        }

        public async Task<Responses.Photos.UploadPhotoResponse> UploadPhotoAsync(
            string uploadUrl,
            string imageFormat,
            byte[] fileStream,
            CancellationToken cancellationToken = default)
        {
            var content = GetUploadContent("photo", imageFormat, fileStream);

            return await SendRequestAsync<Responses.Photos.UploadPhotoResponse>(uploadUrl, HttpMethod.Post, content, cancellationToken);
        }

        public async Task<Responses.Photos.UploadDocumentResponse> UploadDocumentAsync(
            string uploadUrl,
            string imageFormat,
            byte[] fileStream,
            CancellationToken cancellationToken = default)
        {
            var content = GetUploadContent("file", imageFormat, fileStream);

            return await SendRequestAsync<Responses.Photos.UploadDocumentResponse>(uploadUrl, HttpMethod.Post, content, cancellationToken);
        }

        private MultipartFormDataContent GetUploadContent(
            string fieldName,
            string imageFormat,
            params byte[][] files)
        {
            var formData = new MultipartFormDataContent();
            for (var i = 1; i <= files.Length; i++)
            {
                var payload = files[i - 1];
                var fileContent = new ByteArrayContent(payload);
                var fileName = files.Length > 1 ? $"{fieldName}{i}" : fieldName;
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                formData.Add(fileContent, fileName, $"{fileName}.{imageFormat}");
            }

            return formData;
        }

        private Task<T> SendRequestAsync<T>(
            string requestUri,
            HttpMethod httpMethod,
            CancellationToken cancellationToken = default) =>
            SendRequestAsync<T>(requestUri, httpMethod, null, cancellationToken);
        

        private async Task<T> SendRequestAsync<T>(
            string requestUri,
            HttpMethod httpMethod,
            HttpContent? httpContent = null,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(httpMethod, requestUri);
            request.Content = httpContent;

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            response.EnsureSuccessStatusCode();
            var jsonDocument = JsonDocument.Parse(content);

            // [2022-03-12 00:26:50] [System.Net.Http.HttpClient.Default.LogicalHandler] [Information] End processing HTTP request after 33.5608ms - 200
            // { "error":{ "error_code":5,"error_msg":"User authorization failed: invalid access_token (4).","request_params":[{ "key":"v","value":"5.131"},{ "key":"group_id","value":"211186044"},{ "key":"method","value":"photos.getWallUploadServer"},{ "key":"oauth","value":"1"}]} }
            if (jsonDocument.RootElement.TryGetProperty("error", out _))
            {
                var error = jsonDocument.Deserialize<VkError>(_jsonSerializerOptions);
                if (error == null)
                    throw new Exception($"Unexpected response was returned by VK API. Response: {content}");

                throw new Exception($"Error code {error.Error.ErrorCode} - {error.Error.ErrorMsg}");
            }

            var result = jsonDocument.Deserialize<T>(_jsonSerializerOptions)
                         ?? throw new Exception($"Can't deserialize {typeof(T).Name} response of VK API");

            return result;
        }
    }
}