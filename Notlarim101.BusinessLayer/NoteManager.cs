using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notlarim101.BusinessLayer.Abstract;
using Notlarim101.Core.DataAccess;
using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;
using Notlarim101.Entity.Messages;

namespace Notlarim101.BusinessLayer
{
    public class NoteManager : ManagerBase<Note>
    {
        BusinessLayerResult<Note> res = new BusinessLayerResult<Note>();

        //    public List<Note> GetAllNotes()
        //    {
        //        return rnote.List();
        //    }

    }
}