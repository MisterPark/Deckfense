namespace GoblinGames.Core
{
    public abstract class Singleton<T> where T : Singleton<T> , new()
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        // TODO : ��ӹ��� Ŭ���� ���ο��� �ν��Ͻ��� ������ �����Ǵ� ������ ����. �����ؾ� ��.
        protected Singleton() { }
    }
}