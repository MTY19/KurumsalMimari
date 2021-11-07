using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entitities.Concrete;

namespace Business.Abstract
{
    public interface ICategroyService
    {
        IDataResult<List<Category>> GetAll();
    }
}
