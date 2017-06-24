///////////////////////////////////////////////////////////
//Name:框架模型-工人实体集合类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System.Collections;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 工人实体集合类
    /// </summary>
    public class WorkerCollection : IEnumerable
    {
        Hashtable _items = new Hashtable();
        /// <summary>
        /// 工人实体集合类
        /// </summary>
        public WorkerCollection() { }

        /// <summary>
        /// 工人实体集合类
        /// </summary>
        public WorkerCollection(IWorkRoom workRoom)
        {
            WorkRoom = workRoom;
        }

        /// <summary>
        /// 添加工人
        /// </summary>
        /// <param name="worker">工人实体类</param>
        public void Add(IWorker worker)
        {
            if (_items.ContainsKey(worker.Name))
                return;
            _items.Add(worker.Name, worker);
        }

        /// <summary>
        /// 获取或设置工作室实体类
        /// </summary>
        public IWorkRoom WorkRoom
        {
            get; set;
        }

        /// <summary>
        /// 获取或设置工人数量
        /// </summary>
        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// 删除特定工人
        /// </summary>
        /// <param name="worker"></param>
        public void Remove(IWorker worker)
        {
            Remove(worker.Name);
        }

        /// <summary>
        /// 删除特定名字的工人
        /// </summary>
        /// <param name="workerName"></param>
        public void Remove(string workerName)
        {
            if (_items.ContainsKey(workerName))
            {
                _items.Remove(workerName);
            }
        }

        /// <summary>
        /// 重置工人实体集合
        /// </summary>
        public void Reset()
        {
            _items.Clear(); _items = new Hashtable();
        }

        /// <summary>
        /// 获取工人实体枚举对象
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }
    }
}
