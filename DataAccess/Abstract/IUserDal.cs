using Core.DataAccess;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUserDal:IEntityRepository<User>
    {
        //bu bizim için bir join operasyonu
        List<OperationClaim> GetClaims(User user);
    }
}
