﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NewLife.Net.Http
{
    /// <summary>Http缓存。以Url作为缓存键</summary>
    class HttpCache
    {
        #region 属性
        private Dictionary<String, HttpCacheItem> _Items;
        /// <summary>缓存项</summary>
        public Dictionary<String, HttpCacheItem> Items { get { return _Items ?? (_Items = new Dictionary<string, HttpCacheItem>(StringComparer.OrdinalIgnoreCase)); } set { _Items = value; } }
        #endregion

        #region 方法
        public HttpCacheItem GetItem(String url)
        {
            HttpCacheItem item = null;
            if (!Items.TryGetValue(url, out item)) return null;

            // 移除过期
            if (item.ExpiredTime < DateTime.Now)
            {
                Items.Remove(url);
                item = null;
            }

            return item;
        }

        public HttpCacheItem Add(HttpHeader request)
        {
            var url = request.Url.ToString();
            var item = new HttpCacheItem() { Url = url, Request = request };
            Items[url] = item;

            return item;
        }
        #endregion
    }

    /// <summary>Http缓存项。</summary>
    class HttpCacheItem
    {
        #region 属性
        private String _Url;
        /// <summary>网址</summary>
        public String Url { get { return _Url; } set { _Url = value; } }

        private HttpHeader _Request;
        /// <summary>请求</summary>
        public HttpHeader Request { get { return _Request; } set { _Request = value; } }

        private HttpHeader _Response;
        /// <summary>响应</summary>
        public HttpHeader Response { get { return _Response; } set { _Response = value; } }

        private Stream _Stream;
        /// <summary>数据流</summary>
        public Stream Stream { get { return _Stream ?? (_Stream = new MemoryStream()); } set { _Stream = value; } }

        private DateTime _StartTime = DateTime.Now;
        /// <summary>开始时间</summary>
        public DateTime StartTime { get { return _StartTime; } set { _StartTime = value; } }

        private DateTime _ExpiredTime = DateTime.Now.AddMinutes(10);
        /// <summary>到期时间</summary>
        public DateTime ExpiredTime { get { return _ExpiredTime; } set { _ExpiredTime = value; } }
        #endregion
    }
}