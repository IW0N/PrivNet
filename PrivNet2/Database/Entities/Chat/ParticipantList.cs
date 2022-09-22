namespace Server.Database.Entities.ChatEnv
{
    public class ParticipantList:List<User>
    {
        readonly bool extensiable;
        /*
         * if count equals -1, than list is extensiable
         */
        public ParticipantList(int count)
        {
            if (count > -1)
            {
                for (int x=0;x<count;x++)
                    Add(null);
                extensiable = false;
            }
            else
                extensiable = true;
        }
        private void ExecuteBaseFunction(Delegate @delegate,params object[] input)
        {
            if (extensiable)
                @delegate.DynamicInvoke(input);
            else
                throw new IndexOutOfRangeException("List is fixed");
        }
        public new void Add(User user)
        => ExecuteBaseFunction(base.Add, user);
        public new void AddRange(IEnumerable<User> users)
        => ExecuteBaseFunction(base.AddRange, users);
        public new void RemoveAt(int index) => ExecuteBaseFunction(base.RemoveAt, index);
        public new void Remove(User user) => ExecuteBaseFunction(base.Remove, user);
        public new void RemoveRange(int index, int count) =>
            ExecuteBaseFunction(base.RemoveRange, index, count);
        public new void RemoveAll(Predicate<User> match) =>
            ExecuteBaseFunction(base.RemoveAll, match);
    }
}
