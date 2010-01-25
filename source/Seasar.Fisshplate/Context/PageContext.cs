using System;
using System.Collections.Generic;

using System.Text;

namespace Seasar.Fisshplate.Context
{
    /// <summary>
    /// シート単位の値を保管しています。
    /// </summary>
    public class PageContext
    {
        private int _pageNum = 1;

        /// <summary>
        /// ページ番号
        /// </summary>
        public int PageNum
        {
            get { return _pageNum; }
            set { _pageNum = value; }
        }

        /// <summary>
        /// ページ番号を加算します
        /// </summary>
        public void AddPageNum()
        {
            _pageNum++;
        }


    }
}
