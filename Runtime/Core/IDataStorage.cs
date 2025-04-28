namespace CipherSphere.Runtime.Core
{
    public interface IDataStorage
    {
        /// <summary>
        /// セーブデータが存在するか
        /// </summary>
        /// <returns></returns>
        bool Exists();
        
        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        void Save<T>(T data);
        
        /// <summary>
        /// ロードする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Load<T>();
        
        /// <summary>
        /// 削除する
        /// </summary>
        void Delete();
    }
}