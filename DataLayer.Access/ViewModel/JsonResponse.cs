﻿using System.Collections.Generic;

namespace DataLayer.Access.ViewModel
{
    public class JsonResponse
    {
        public JsonResponse()
        {
            InfoData = new Dictionary<string, string>();
            Status = JsonResponseStatus.Success;
        }
        public JsonResponseStatus Status { get; set; }
        public Dictionary<string, string> InfoData { get; set; }
    }

    public class JsonResponse<TResult> : JsonResponse
    {
        public TResult TResult001 { get; set; }
    }

    public static class JsonResponseHandler
    {
        public static void AddError(this JsonResponse jsonResponse, string key, string error)
        {
            jsonResponse.Status = JsonResponseStatus.HaveError;
            jsonResponse.InfoData[key] = error;
        }
    }

    public enum JsonResponseStatus : uint
    {
        Success,
        HaveError,
        NotFound
    }

}