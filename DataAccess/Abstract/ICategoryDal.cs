using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Entitities.Concrete;

namespace DataAccess.Abstract
{
    public interface ICategoryDal:IEntityRepository<Category>
    {
    }
}
