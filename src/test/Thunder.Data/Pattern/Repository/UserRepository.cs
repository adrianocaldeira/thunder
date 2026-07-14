using Thunder.Data.Pattern.Repository.Domain;
using Thunder.NHibernate.Pattern;

namespace Thunder.Data.Pattern.Repository
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
    }
}
