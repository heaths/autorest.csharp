// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using CognitiveSearch.Models;

namespace CognitiveSearch
{
    internal partial class DocumentsOperations
    {
        private string searchServiceName;
        private string indexName;
        private string searchDnsSuffix;
        private string ApiVersion;
        private ClientDiagnostics clientDiagnostics;
        private HttpPipeline pipeline;
        /// <summary> Initializes a new instance of DocumentsOperations. </summary>
        public DocumentsOperations(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, string searchServiceName, string indexName, string searchDnsSuffix = "search.windows.net", string ApiVersion = "2019-05-06")
        {
            if (searchServiceName == null)
            {
                throw new ArgumentNullException(nameof(searchServiceName));
            }
            if (indexName == null)
            {
                throw new ArgumentNullException(nameof(indexName));
            }
            if (searchDnsSuffix == null)
            {
                throw new ArgumentNullException(nameof(searchDnsSuffix));
            }
            if (ApiVersion == null)
            {
                throw new ArgumentNullException(nameof(ApiVersion));
            }

            this.searchServiceName = searchServiceName;
            this.indexName = indexName;
            this.searchDnsSuffix = searchDnsSuffix;
            this.ApiVersion = ApiVersion;
            this.clientDiagnostics = clientDiagnostics;
            this.pipeline = pipeline;
        }
        internal HttpMessage CreateCountRequest(Guid? clientRequestId)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs/$count", false);
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            return message;
        }
        /// <summary> Queries the number of documents in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<long>> CountAsync(Guid? clientRequestId, CancellationToken cancellationToken = default)
        {

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.Count");
            scope.Start();
            try
            {
                using var message = CreateCountRequest(clientRequestId);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = document.RootElement.GetInt64();
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Queries the number of documents in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<long> Count(Guid? clientRequestId, CancellationToken cancellationToken = default)
        {

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.Count");
            scope.Start();
            try
            {
                using var message = CreateCountRequest(clientRequestId);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = document.RootElement.GetInt64();
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateSearchGetRequest(string? searchText, bool? includeTotalResultCount, IEnumerable<string>? facets, string? filter, IEnumerable<string>? highlightFields, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? orderBy, QueryType? queryType, IEnumerable<string>? scoringParameters, string? scoringProfile, IEnumerable<string>? searchFields, SearchMode? searchMode, IEnumerable<string>? select, int? skip, int? top, Guid? clientRequestId)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs", false);
            if (searchText != null)
            {
                uri.AppendQuery("search", searchText, true);
            }
            if (includeTotalResultCount != null)
            {
                uri.AppendQuery("$count", includeTotalResultCount.Value, true);
            }
            if (facets != null)
            {
                uri.AppendQueryDelimited("facet", facets, ",", true);
            }
            if (filter != null)
            {
                uri.AppendQuery("$filter", filter, true);
            }
            if (highlightFields != null)
            {
                uri.AppendQueryDelimited("highlight", highlightFields, ",", true);
            }
            if (highlightPostTag != null)
            {
                uri.AppendQuery("highlightPostTag", highlightPostTag, true);
            }
            if (highlightPreTag != null)
            {
                uri.AppendQuery("highlightPreTag", highlightPreTag, true);
            }
            if (minimumCoverage != null)
            {
                uri.AppendQuery("minimumCoverage", minimumCoverage.Value, true);
            }
            if (orderBy != null)
            {
                uri.AppendQueryDelimited("$orderby", orderBy, ",", true);
            }
            if (queryType != null)
            {
                uri.AppendQuery("queryType", queryType.Value, true);
            }
            if (scoringParameters != null)
            {
                uri.AppendQueryDelimited("scoringParameter", scoringParameters, ",", true);
            }
            if (scoringProfile != null)
            {
                uri.AppendQuery("scoringProfile", scoringProfile, true);
            }
            if (searchFields != null)
            {
                uri.AppendQueryDelimited("searchFields", searchFields, ",", true);
            }
            if (searchMode != null)
            {
                uri.AppendQuery("searchMode", searchMode.Value, true);
            }
            if (select != null)
            {
                uri.AppendQueryDelimited("$select", select, ",", true);
            }
            if (skip != null)
            {
                uri.AppendQuery("$skip", skip.Value, true);
            }
            if (top != null)
            {
                uri.AppendQuery("$top", top.Value, true);
            }
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            return message;
        }
        /// <summary> Searches for documents in the index. </summary>
        /// <param name="searchText"> A full-text search query expression; Use &quot;*&quot; or omit this parameter to match all documents. </param>
        /// <param name="includeTotalResultCount"> A value that specifies whether to fetch the total count of results. Default is false. Setting this value to true may have a performance impact. Note that the count returned is an approximation. </param>
        /// <param name="facets"> The list of facet expressions to apply to the search query. Each facet expression contains a field name, optionally followed by a comma-separated list of name:value pairs. </param>
        /// <param name="filter"> The OData $filter expression to apply to the search query. </param>
        /// <param name="highlightFields"> The list of field names to use for hit highlights. Only searchable fields can be used for hit highlighting. </param>
        /// <param name="highlightPostTag"> A string tag that is appended to hit highlights. Must be set with highlightPreTag. Default is &amp;lt;/em&amp;gt;. </param>
        /// <param name="highlightPreTag"> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. Default is &amp;lt;em&amp;gt;. </param>
        /// <param name="minimumCoverage"> A number between 0 and 100 indicating the percentage of the index that must be covered by a search query in order for the query to be reported as a success. This parameter can be useful for ensuring search availability even for services with only one replica. The default is 100. </param>
        /// <param name="orderBy"> The list of OData $orderby expressions by which to sort the results. Each expression can be either a field name or a call to either the geo.distance() or the search.score() functions. Each expression can be followed by asc to indicate ascending, and desc to indicate descending. The default is ascending order. Ties will be broken by the match scores of documents. If no OrderBy is specified, the default sort order is descending by document match score. There can be at most 32 $orderby clauses. </param>
        /// <param name="queryType"> A value that specifies the syntax of the search query. The default is &apos;simple&apos;. Use &apos;full&apos; if your query uses the Lucene query syntax. </param>
        /// <param name="scoringParameters"> The list of parameter values to be used in scoring functions (for example, referencePointParameter) using the format name-values. For example, if the scoring profile defines a function with a parameter called &apos;mylocation&apos; the parameter string would be &quot;mylocation--122.2,44.8&quot; (without the quotes). </param>
        /// <param name="scoringProfile"> The name of a scoring profile to evaluate match scores for matching documents in order to sort the results. </param>
        /// <param name="searchFields"> The list of field names to which to scope the full-text search. When using fielded search (fieldName:searchExpression) in a full Lucene query, the field names of each fielded search expression take precedence over any field names listed in this parameter. </param>
        /// <param name="searchMode"> A value that specifies whether any or all of the search terms must be matched in order to count the document as a match. </param>
        /// <param name="select"> The list of fields to retrieve. If unspecified, all fields marked as retrievable in the schema are included. </param>
        /// <param name="skip"> The number of search results to skip. This value cannot be greater than 100,000. If you need to scan documents in sequence, but cannot use $skip due to this limitation, consider using $orderby on a totally-ordered key and $filter with a range query instead. </param>
        /// <param name="top"> The number of search results to retrieve. This can be used in conjunction with $skip to implement client-side paging of search results. If results are truncated due to server-side paging, the response will include a continuation token that can be used to issue another Search request for the next page of results. </param>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<SearchDocumentsResult>> SearchGetAsync(string? searchText, bool? includeTotalResultCount, IEnumerable<string>? facets, string? filter, IEnumerable<string>? highlightFields, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? orderBy, QueryType? queryType, IEnumerable<string>? scoringParameters, string? scoringProfile, IEnumerable<string>? searchFields, SearchMode? searchMode, IEnumerable<string>? select, int? skip, int? top, Guid? clientRequestId, CancellationToken cancellationToken = default)
        {

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SearchGet");
            scope.Start();
            try
            {
                using var message = CreateSearchGetRequest(searchText, includeTotalResultCount, facets, filter, highlightFields, highlightPostTag, highlightPreTag, minimumCoverage, orderBy, queryType, scoringParameters, scoringProfile, searchFields, searchMode, select, skip, top, clientRequestId);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = SearchDocumentsResult.DeserializeSearchDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Searches for documents in the index. </summary>
        /// <param name="searchText"> A full-text search query expression; Use &quot;*&quot; or omit this parameter to match all documents. </param>
        /// <param name="includeTotalResultCount"> A value that specifies whether to fetch the total count of results. Default is false. Setting this value to true may have a performance impact. Note that the count returned is an approximation. </param>
        /// <param name="facets"> The list of facet expressions to apply to the search query. Each facet expression contains a field name, optionally followed by a comma-separated list of name:value pairs. </param>
        /// <param name="filter"> The OData $filter expression to apply to the search query. </param>
        /// <param name="highlightFields"> The list of field names to use for hit highlights. Only searchable fields can be used for hit highlighting. </param>
        /// <param name="highlightPostTag"> A string tag that is appended to hit highlights. Must be set with highlightPreTag. Default is &amp;lt;/em&amp;gt;. </param>
        /// <param name="highlightPreTag"> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. Default is &amp;lt;em&amp;gt;. </param>
        /// <param name="minimumCoverage"> A number between 0 and 100 indicating the percentage of the index that must be covered by a search query in order for the query to be reported as a success. This parameter can be useful for ensuring search availability even for services with only one replica. The default is 100. </param>
        /// <param name="orderBy"> The list of OData $orderby expressions by which to sort the results. Each expression can be either a field name or a call to either the geo.distance() or the search.score() functions. Each expression can be followed by asc to indicate ascending, and desc to indicate descending. The default is ascending order. Ties will be broken by the match scores of documents. If no OrderBy is specified, the default sort order is descending by document match score. There can be at most 32 $orderby clauses. </param>
        /// <param name="queryType"> A value that specifies the syntax of the search query. The default is &apos;simple&apos;. Use &apos;full&apos; if your query uses the Lucene query syntax. </param>
        /// <param name="scoringParameters"> The list of parameter values to be used in scoring functions (for example, referencePointParameter) using the format name-values. For example, if the scoring profile defines a function with a parameter called &apos;mylocation&apos; the parameter string would be &quot;mylocation--122.2,44.8&quot; (without the quotes). </param>
        /// <param name="scoringProfile"> The name of a scoring profile to evaluate match scores for matching documents in order to sort the results. </param>
        /// <param name="searchFields"> The list of field names to which to scope the full-text search. When using fielded search (fieldName:searchExpression) in a full Lucene query, the field names of each fielded search expression take precedence over any field names listed in this parameter. </param>
        /// <param name="searchMode"> A value that specifies whether any or all of the search terms must be matched in order to count the document as a match. </param>
        /// <param name="select"> The list of fields to retrieve. If unspecified, all fields marked as retrievable in the schema are included. </param>
        /// <param name="skip"> The number of search results to skip. This value cannot be greater than 100,000. If you need to scan documents in sequence, but cannot use $skip due to this limitation, consider using $orderby on a totally-ordered key and $filter with a range query instead. </param>
        /// <param name="top"> The number of search results to retrieve. This can be used in conjunction with $skip to implement client-side paging of search results. If results are truncated due to server-side paging, the response will include a continuation token that can be used to issue another Search request for the next page of results. </param>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<SearchDocumentsResult> SearchGet(string? searchText, bool? includeTotalResultCount, IEnumerable<string>? facets, string? filter, IEnumerable<string>? highlightFields, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? orderBy, QueryType? queryType, IEnumerable<string>? scoringParameters, string? scoringProfile, IEnumerable<string>? searchFields, SearchMode? searchMode, IEnumerable<string>? select, int? skip, int? top, Guid? clientRequestId, CancellationToken cancellationToken = default)
        {

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SearchGet");
            scope.Start();
            try
            {
                using var message = CreateSearchGetRequest(searchText, includeTotalResultCount, facets, filter, highlightFields, highlightPostTag, highlightPreTag, minimumCoverage, orderBy, queryType, scoringParameters, scoringProfile, searchFields, searchMode, select, skip, top, clientRequestId);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = SearchDocumentsResult.DeserializeSearchDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateSearchPostRequest(Guid? clientRequestId, SearchRequest searchRequest)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs/search.post.search", false);
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            request.Headers.Add("Content-Type", "application/json");
            using var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(searchRequest);
            request.Content = content;
            return message;
        }
        /// <summary> Searches for documents in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="searchRequest"> The definition of the Search request. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<SearchDocumentsResult>> SearchPostAsync(Guid? clientRequestId, SearchRequest searchRequest, CancellationToken cancellationToken = default)
        {
            if (searchRequest == null)
            {
                throw new ArgumentNullException(nameof(searchRequest));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SearchPost");
            scope.Start();
            try
            {
                using var message = CreateSearchPostRequest(clientRequestId, searchRequest);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = SearchDocumentsResult.DeserializeSearchDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Searches for documents in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="searchRequest"> The definition of the Search request. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<SearchDocumentsResult> SearchPost(Guid? clientRequestId, SearchRequest searchRequest, CancellationToken cancellationToken = default)
        {
            if (searchRequest == null)
            {
                throw new ArgumentNullException(nameof(searchRequest));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SearchPost");
            scope.Start();
            try
            {
                using var message = CreateSearchPostRequest(clientRequestId, searchRequest);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = SearchDocumentsResult.DeserializeSearchDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateGetRequest(string key, IEnumerable<string>? selectedFields, Guid? clientRequestId)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs('", false);
            uri.AppendPath(key, true);
            uri.AppendPath("')", false);
            if (selectedFields != null)
            {
                uri.AppendQueryDelimited("$select", selectedFields, ",", true);
            }
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            return message;
        }
        /// <summary> Retrieves a document from the index. </summary>
        /// <param name="key"> The key of the document to retrieve. </param>
        /// <param name="selectedFields"> List of field names to retrieve for the document; Any field not retrieved will be missing from the returned document. </param>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<ComponentsSchemasFacetresultAdditionalproperties>> GetAsync(string key, IEnumerable<string>? selectedFields, Guid? clientRequestId, CancellationToken cancellationToken = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.Get");
            scope.Start();
            try
            {
                using var message = CreateGetRequest(key, selectedFields, clientRequestId);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = ComponentsSchemasFacetresultAdditionalproperties.DeserializeComponentsSchemasFacetresultAdditionalproperties(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Retrieves a document from the index. </summary>
        /// <param name="key"> The key of the document to retrieve. </param>
        /// <param name="selectedFields"> List of field names to retrieve for the document; Any field not retrieved will be missing from the returned document. </param>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<ComponentsSchemasFacetresultAdditionalproperties> Get(string key, IEnumerable<string>? selectedFields, Guid? clientRequestId, CancellationToken cancellationToken = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.Get");
            scope.Start();
            try
            {
                using var message = CreateGetRequest(key, selectedFields, clientRequestId);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = ComponentsSchemasFacetresultAdditionalproperties.DeserializeComponentsSchemasFacetresultAdditionalproperties(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateSuggestGetRequest(string searchText, string suggesterName, string? filter, bool? useFuzzyMatching, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? orderBy, IEnumerable<string>? searchFields, IEnumerable<string>? select, int? top, Guid? clientRequestId)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs/search.suggest", false);
            uri.AppendQuery("search", searchText, true);
            uri.AppendQuery("suggesterName", suggesterName, true);
            if (filter != null)
            {
                uri.AppendQuery("$filter", filter, true);
            }
            if (useFuzzyMatching != null)
            {
                uri.AppendQuery("fuzzy", useFuzzyMatching.Value, true);
            }
            if (highlightPostTag != null)
            {
                uri.AppendQuery("highlightPostTag", highlightPostTag, true);
            }
            if (highlightPreTag != null)
            {
                uri.AppendQuery("highlightPreTag", highlightPreTag, true);
            }
            if (minimumCoverage != null)
            {
                uri.AppendQuery("minimumCoverage", minimumCoverage.Value, true);
            }
            if (orderBy != null)
            {
                uri.AppendQueryDelimited("$orderby", orderBy, ",", true);
            }
            if (searchFields != null)
            {
                uri.AppendQueryDelimited("searchFields", searchFields, ",", true);
            }
            if (select != null)
            {
                uri.AppendQueryDelimited("$select", select, ",", true);
            }
            if (top != null)
            {
                uri.AppendQuery("$top", top.Value, true);
            }
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            return message;
        }
        /// <summary> Suggests documents in the index that match the given partial query text. </summary>
        /// <param name="searchText"> The search text to use to suggest documents. Must be at least 1 character, and no more than 100 characters. </param>
        /// <param name="suggesterName"> The name of the suggester as specified in the suggesters collection that&apos;s part of the index definition. </param>
        /// <param name="filter"> An OData expression that filters the documents considered for suggestions. </param>
        /// <param name="useFuzzyMatching"> A value indicating whether to use fuzzy matching for the suggestions query. Default is false. When set to true, the query will find terms even if there&apos;s a substituted or missing character in the search text. While this provides a better experience in some scenarios, it comes at a performance cost as fuzzy suggestions queries are slower and consume more resources. </param>
        /// <param name="highlightPostTag"> A string tag that is appended to hit highlights. Must be set with highlightPreTag. If omitted, hit highlighting of suggestions is disabled. </param>
        /// <param name="highlightPreTag"> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. If omitted, hit highlighting of suggestions is disabled. </param>
        /// <param name="minimumCoverage"> A number between 0 and 100 indicating the percentage of the index that must be covered by a suggestions query in order for the query to be reported as a success. This parameter can be useful for ensuring search availability even for services with only one replica. The default is 80. </param>
        /// <param name="orderBy"> The list of OData $orderby expressions by which to sort the results. Each expression can be either a field name or a call to either the geo.distance() or the search.score() functions. Each expression can be followed by asc to indicate ascending, or desc to indicate descending. The default is ascending order. Ties will be broken by the match scores of documents. If no $orderby is specified, the default sort order is descending by document match score. There can be at most 32 $orderby clauses. </param>
        /// <param name="searchFields"> The list of field names to search for the specified search text. Target fields must be included in the specified suggester. </param>
        /// <param name="select"> The list of fields to retrieve. If unspecified, only the key field will be included in the results. </param>
        /// <param name="top"> The number of suggestions to retrieve. The value must be a number between 1 and 100. The default is 5. </param>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<SuggestDocumentsResult>> SuggestGetAsync(string searchText, string suggesterName, string? filter, bool? useFuzzyMatching, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? orderBy, IEnumerable<string>? searchFields, IEnumerable<string>? select, int? top, Guid? clientRequestId, CancellationToken cancellationToken = default)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText));
            }
            if (suggesterName == null)
            {
                throw new ArgumentNullException(nameof(suggesterName));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SuggestGet");
            scope.Start();
            try
            {
                using var message = CreateSuggestGetRequest(searchText, suggesterName, filter, useFuzzyMatching, highlightPostTag, highlightPreTag, minimumCoverage, orderBy, searchFields, select, top, clientRequestId);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = SuggestDocumentsResult.DeserializeSuggestDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Suggests documents in the index that match the given partial query text. </summary>
        /// <param name="searchText"> The search text to use to suggest documents. Must be at least 1 character, and no more than 100 characters. </param>
        /// <param name="suggesterName"> The name of the suggester as specified in the suggesters collection that&apos;s part of the index definition. </param>
        /// <param name="filter"> An OData expression that filters the documents considered for suggestions. </param>
        /// <param name="useFuzzyMatching"> A value indicating whether to use fuzzy matching for the suggestions query. Default is false. When set to true, the query will find terms even if there&apos;s a substituted or missing character in the search text. While this provides a better experience in some scenarios, it comes at a performance cost as fuzzy suggestions queries are slower and consume more resources. </param>
        /// <param name="highlightPostTag"> A string tag that is appended to hit highlights. Must be set with highlightPreTag. If omitted, hit highlighting of suggestions is disabled. </param>
        /// <param name="highlightPreTag"> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. If omitted, hit highlighting of suggestions is disabled. </param>
        /// <param name="minimumCoverage"> A number between 0 and 100 indicating the percentage of the index that must be covered by a suggestions query in order for the query to be reported as a success. This parameter can be useful for ensuring search availability even for services with only one replica. The default is 80. </param>
        /// <param name="orderBy"> The list of OData $orderby expressions by which to sort the results. Each expression can be either a field name or a call to either the geo.distance() or the search.score() functions. Each expression can be followed by asc to indicate ascending, or desc to indicate descending. The default is ascending order. Ties will be broken by the match scores of documents. If no $orderby is specified, the default sort order is descending by document match score. There can be at most 32 $orderby clauses. </param>
        /// <param name="searchFields"> The list of field names to search for the specified search text. Target fields must be included in the specified suggester. </param>
        /// <param name="select"> The list of fields to retrieve. If unspecified, only the key field will be included in the results. </param>
        /// <param name="top"> The number of suggestions to retrieve. The value must be a number between 1 and 100. The default is 5. </param>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<SuggestDocumentsResult> SuggestGet(string searchText, string suggesterName, string? filter, bool? useFuzzyMatching, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? orderBy, IEnumerable<string>? searchFields, IEnumerable<string>? select, int? top, Guid? clientRequestId, CancellationToken cancellationToken = default)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText));
            }
            if (suggesterName == null)
            {
                throw new ArgumentNullException(nameof(suggesterName));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SuggestGet");
            scope.Start();
            try
            {
                using var message = CreateSuggestGetRequest(searchText, suggesterName, filter, useFuzzyMatching, highlightPostTag, highlightPreTag, minimumCoverage, orderBy, searchFields, select, top, clientRequestId);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = SuggestDocumentsResult.DeserializeSuggestDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateSuggestPostRequest(Guid? clientRequestId, SuggestRequest suggestRequest)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs/search.post.suggest", false);
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            request.Headers.Add("Content-Type", "application/json");
            using var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(suggestRequest);
            request.Content = content;
            return message;
        }
        /// <summary> Suggests documents in the index that match the given partial query text. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="suggestRequest"> The Suggest request. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<SuggestDocumentsResult>> SuggestPostAsync(Guid? clientRequestId, SuggestRequest suggestRequest, CancellationToken cancellationToken = default)
        {
            if (suggestRequest == null)
            {
                throw new ArgumentNullException(nameof(suggestRequest));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SuggestPost");
            scope.Start();
            try
            {
                using var message = CreateSuggestPostRequest(clientRequestId, suggestRequest);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = SuggestDocumentsResult.DeserializeSuggestDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Suggests documents in the index that match the given partial query text. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="suggestRequest"> The Suggest request. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<SuggestDocumentsResult> SuggestPost(Guid? clientRequestId, SuggestRequest suggestRequest, CancellationToken cancellationToken = default)
        {
            if (suggestRequest == null)
            {
                throw new ArgumentNullException(nameof(suggestRequest));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.SuggestPost");
            scope.Start();
            try
            {
                using var message = CreateSuggestPostRequest(clientRequestId, suggestRequest);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = SuggestDocumentsResult.DeserializeSuggestDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateIndexRequest(Guid? clientRequestId, IndexBatch batch)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs/search.index", false);
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            request.Headers.Add("Content-Type", "application/json");
            using var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(batch);
            request.Content = content;
            return message;
        }
        /// <summary> Sends a batch of document write actions to the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="batch"> The batch of index actions. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<IndexDocumentsResult>> IndexAsync(Guid? clientRequestId, IndexBatch batch, CancellationToken cancellationToken = default)
        {
            if (batch == null)
            {
                throw new ArgumentNullException(nameof(batch));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.Index");
            scope.Start();
            try
            {
                using var message = CreateIndexRequest(clientRequestId, batch);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = IndexDocumentsResult.DeserializeIndexDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Sends a batch of document write actions to the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="batch"> The batch of index actions. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<IndexDocumentsResult> Index(Guid? clientRequestId, IndexBatch batch, CancellationToken cancellationToken = default)
        {
            if (batch == null)
            {
                throw new ArgumentNullException(nameof(batch));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.Index");
            scope.Start();
            try
            {
                using var message = CreateIndexRequest(clientRequestId, batch);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = IndexDocumentsResult.DeserializeIndexDocumentsResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateAutocompleteGetRequest(Guid? clientRequestId, string searchText, string suggesterName, AutocompleteMode? autocompleteMode, string? filter, bool? useFuzzyMatching, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? searchFields, int? top)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs/search.autocomplete", false);
            uri.AppendQuery("api-version", ApiVersion, true);
            uri.AppendQuery("search", searchText, true);
            uri.AppendQuery("suggesterName", suggesterName, true);
            if (autocompleteMode != null)
            {
                uri.AppendQuery("autocompleteMode", autocompleteMode.Value, true);
            }
            if (filter != null)
            {
                uri.AppendQuery("$filter", filter, true);
            }
            if (useFuzzyMatching != null)
            {
                uri.AppendQuery("fuzzy", useFuzzyMatching.Value, true);
            }
            if (highlightPostTag != null)
            {
                uri.AppendQuery("highlightPostTag", highlightPostTag, true);
            }
            if (highlightPreTag != null)
            {
                uri.AppendQuery("highlightPreTag", highlightPreTag, true);
            }
            if (minimumCoverage != null)
            {
                uri.AppendQuery("minimumCoverage", minimumCoverage.Value, true);
            }
            if (searchFields != null)
            {
                uri.AppendQueryDelimited("searchFields", searchFields, ",", true);
            }
            if (top != null)
            {
                uri.AppendQuery("$top", top.Value, true);
            }
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            return message;
        }
        /// <summary> Autocompletes incomplete query terms based on input text and matching terms in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="searchText"> The search text to use to suggest documents. Must be at least 1 character, and no more than 100 characters. </param>
        /// <param name="suggesterName"> The name of the suggester as specified in the suggesters collection that&apos;s part of the index definition. </param>
        /// <param name="autocompleteMode"> Specifies the mode for Autocomplete. The default is &apos;oneTerm&apos;. Use &apos;twoTerms&apos; to get shingles and &apos;oneTermWithContext&apos; to use the current context while producing auto-completed terms. </param>
        /// <param name="filter"> An OData expression that filters the documents used to produce completed terms for the Autocomplete result. </param>
        /// <param name="useFuzzyMatching"> A value indicating whether to use fuzzy matching for the autocomplete query. Default is false. When set to true, the query will find terms even if there&apos;s a substituted or missing character in the search text. While this provides a better experience in some scenarios, it comes at a performance cost as fuzzy autocomplete queries are slower and consume more resources. </param>
        /// <param name="highlightPostTag"> A string tag that is appended to hit highlights. Must be set with highlightPreTag. If omitted, hit highlighting is disabled. </param>
        /// <param name="highlightPreTag"> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. If omitted, hit highlighting is disabled. </param>
        /// <param name="minimumCoverage"> A number between 0 and 100 indicating the percentage of the index that must be covered by an autocomplete query in order for the query to be reported as a success. This parameter can be useful for ensuring search availability even for services with only one replica. The default is 80. </param>
        /// <param name="searchFields"> The list of field names to consider when querying for auto-completed terms. Target fields must be included in the specified suggester. </param>
        /// <param name="top"> The number of auto-completed terms to retrieve. This must be a value between 1 and 100. The default is 5. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<AutocompleteResult>> AutocompleteGetAsync(Guid? clientRequestId, string searchText, string suggesterName, AutocompleteMode? autocompleteMode, string? filter, bool? useFuzzyMatching, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? searchFields, int? top, CancellationToken cancellationToken = default)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText));
            }
            if (suggesterName == null)
            {
                throw new ArgumentNullException(nameof(suggesterName));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.AutocompleteGet");
            scope.Start();
            try
            {
                using var message = CreateAutocompleteGetRequest(clientRequestId, searchText, suggesterName, autocompleteMode, filter, useFuzzyMatching, highlightPostTag, highlightPreTag, minimumCoverage, searchFields, top);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = AutocompleteResult.DeserializeAutocompleteResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Autocompletes incomplete query terms based on input text and matching terms in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="searchText"> The search text to use to suggest documents. Must be at least 1 character, and no more than 100 characters. </param>
        /// <param name="suggesterName"> The name of the suggester as specified in the suggesters collection that&apos;s part of the index definition. </param>
        /// <param name="autocompleteMode"> Specifies the mode for Autocomplete. The default is &apos;oneTerm&apos;. Use &apos;twoTerms&apos; to get shingles and &apos;oneTermWithContext&apos; to use the current context while producing auto-completed terms. </param>
        /// <param name="filter"> An OData expression that filters the documents used to produce completed terms for the Autocomplete result. </param>
        /// <param name="useFuzzyMatching"> A value indicating whether to use fuzzy matching for the autocomplete query. Default is false. When set to true, the query will find terms even if there&apos;s a substituted or missing character in the search text. While this provides a better experience in some scenarios, it comes at a performance cost as fuzzy autocomplete queries are slower and consume more resources. </param>
        /// <param name="highlightPostTag"> A string tag that is appended to hit highlights. Must be set with highlightPreTag. If omitted, hit highlighting is disabled. </param>
        /// <param name="highlightPreTag"> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. If omitted, hit highlighting is disabled. </param>
        /// <param name="minimumCoverage"> A number between 0 and 100 indicating the percentage of the index that must be covered by an autocomplete query in order for the query to be reported as a success. This parameter can be useful for ensuring search availability even for services with only one replica. The default is 80. </param>
        /// <param name="searchFields"> The list of field names to consider when querying for auto-completed terms. Target fields must be included in the specified suggester. </param>
        /// <param name="top"> The number of auto-completed terms to retrieve. This must be a value between 1 and 100. The default is 5. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<AutocompleteResult> AutocompleteGet(Guid? clientRequestId, string searchText, string suggesterName, AutocompleteMode? autocompleteMode, string? filter, bool? useFuzzyMatching, string? highlightPostTag, string? highlightPreTag, double? minimumCoverage, IEnumerable<string>? searchFields, int? top, CancellationToken cancellationToken = default)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText));
            }
            if (suggesterName == null)
            {
                throw new ArgumentNullException(nameof(suggesterName));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.AutocompleteGet");
            scope.Start();
            try
            {
                using var message = CreateAutocompleteGetRequest(clientRequestId, searchText, suggesterName, autocompleteMode, filter, useFuzzyMatching, highlightPostTag, highlightPreTag, minimumCoverage, searchFields, top);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = AutocompleteResult.DeserializeAutocompleteResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        internal HttpMessage CreateAutocompletePostRequest(Guid? clientRequestId, AutocompleteRequest autocompleteRequest)
        {
            var message = pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw("https://", false);
            uri.AppendRaw(searchServiceName, false);
            uri.AppendRaw(".", false);
            uri.AppendRaw(searchDnsSuffix, false);
            uri.AppendRaw("/indexes('", false);
            uri.AppendRaw(indexName, true);
            uri.AppendRaw("')", false);
            uri.AppendPath("/docs/search.post.autocomplete", false);
            uri.AppendQuery("api-version", ApiVersion, true);
            request.Uri = uri;
            if (clientRequestId != null)
            {
                request.Headers.Add("client-request-id", clientRequestId.Value);
            }
            request.Headers.Add("Content-Type", "application/json");
            using var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(autocompleteRequest);
            request.Content = content;
            return message;
        }
        /// <summary> Autocompletes incomplete query terms based on input text and matching terms in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="autocompleteRequest"> The definition of the Autocomplete request. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async ValueTask<Response<AutocompleteResult>> AutocompletePostAsync(Guid? clientRequestId, AutocompleteRequest autocompleteRequest, CancellationToken cancellationToken = default)
        {
            if (autocompleteRequest == null)
            {
                throw new ArgumentNullException(nameof(autocompleteRequest));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.AutocompletePost");
            scope.Start();
            try
            {
                using var message = CreateAutocompletePostRequest(clientRequestId, autocompleteRequest);
                await pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                            var value = AutocompleteResult.DeserializeAutocompleteResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw await message.Response.CreateRequestFailedExceptionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
        /// <summary> Autocompletes incomplete query terms based on input text and matching terms in the index. </summary>
        /// <param name="clientRequestId"> The tracking ID sent with the request to help with debugging. </param>
        /// <param name="autocompleteRequest"> The definition of the Autocomplete request. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<AutocompleteResult> AutocompletePost(Guid? clientRequestId, AutocompleteRequest autocompleteRequest, CancellationToken cancellationToken = default)
        {
            if (autocompleteRequest == null)
            {
                throw new ArgumentNullException(nameof(autocompleteRequest));
            }

            using var scope = clientDiagnostics.CreateScope("DocumentsOperations.AutocompletePost");
            scope.Start();
            try
            {
                using var message = CreateAutocompletePostRequest(clientRequestId, autocompleteRequest);
                pipeline.Send(message, cancellationToken);
                switch (message.Response.Status)
                {
                    case 200:
                        {
                            using var document = JsonDocument.Parse(message.Response.ContentStream);
                            var value = AutocompleteResult.DeserializeAutocompleteResult(document.RootElement);
                            return Response.FromValue(value, message.Response);
                        }
                    default:
                        throw message.Response.CreateRequestFailedException();
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
