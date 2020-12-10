using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.UnifiedResponseMessage
{
    public class QueryPageRequest<T>
    {
        #region Constructor
        public QueryPageRequest()
        {

        }

        public QueryPageRequest(T _condition, int _pageIndex, int _pageSize, SortOpt[] _sort = null)
        {
            condition = _condition;
            paging = new PageRequest(_pageSize, _pageIndex);
            //sort = _sort;
        }
        public QueryPageRequest(T _condition, PageRequest _paging, SortOpt[] _sort = null)
        {
            condition = _condition;
            paging = _paging;
            //sort = _sort;
        }

        #endregion
        [JsonRequired]
        public PageRequest paging { get; set; }
        //[JsonRequired]
        //public SortOpt[] sort { get; set; }
        [JsonRequired]
        public T condition { get; set; }
    }
    public class PageRequest
    {
        #region Constructor
        public PageRequest()
        {

        }
        public PageRequest(int _pageSize, int _pageIndex)
        {
            pageSize = _pageSize;
            pageIndex = _pageIndex;
        }
        #endregion

        private int _pagesize;
        public int pageSize
        {
            get
            {
                return _pagesize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("不合法的pagesize");
                }
                _pagesize = value;
            }
        }


        private int _pageindex;
        public int pageIndex
        {
            get
            {
                return _pageindex;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("不合法的pageIndex");
                }
                _pageindex = value;
            }
        }
        [JsonIgnore]
        public int _mySqlLimitStart { get { return (pageIndex - 1) * pageSize; } }
        [JsonIgnore]
        public int _mySqlLimitRows { get { return pageSize; } }
    }

    public class SortOpt
    {
        public string sortField { get; set; }
        public int order { get; set; }
        public int Field { get; set; }
        public bool IsDesc { get; set; }
    }

    public class QueryPageResponse<T>
    {
        #region Constructor
        public QueryPageResponse()
        {

        }
        public QueryPageResponse(IEnumerable<T> _itemList, int _total, int _currentPage, int _pageSize)
        {
            itemList = _itemList;
            paging = new PageResponse(_total, _currentPage, _pageSize);
        }

        public QueryPageResponse(IEnumerable<T> _itemList, PageResponse _paging)
        {
            itemList = _itemList;
            paging = _paging;
        }
        #endregion
        public PageResponse paging { get; set; }
        public IEnumerable<T> itemList { get; set; }
    }
    public class PageResponse
    {
        #region Constructor
        public PageResponse(int _total, int _currentPage, int _pageSize)
        {
            total = _total;
            currentPage = _currentPage;
            pageSize = _pageSize;
            pageCount = (int)Math.Ceiling(_total / (double)(_pageSize <= 0 ? 1 : _pageSize));
        }
        #endregion
        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 当前页索引
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 当页条数
        /// </summary>
        public int pageCount { get; set; }
    }
    public class PageTotals
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public int Total { get; set; }
    }
}
