using LibraryManager.Utility.Enums;
using LibraryManager.Utility.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManager.EntityFramework.Model
{
   /// <summary>
   /// Class Data Access Layer for Author
   /// </summary>
   public class AuthorDAL : IDatabaseAccess<AuthorDTO, int>
   {
      public static AuthorDAL Instance { get => (instance == null) ? new AuthorDAL() : instance; }

      private AuthorDAL()
      {
      }

      public ObservableCollection<Author> GetRawList()
      {
         var rawList = EFProvider.Instance.Database.Authors.Where(x => x.Status == true).ToList();
         var result = new ObservableCollection<Author>();
         foreach (var item in rawList) { result.Add(item); }
         return result;
      }

      public ObservableCollection<AuthorDTO> GetList(StatusFillter fillter = StatusFillter.AllStatus)
      {
         var listAuthorDTO = new ObservableCollection<AuthorDTO>();
         var listRaw = new List<Author>();

         switch (fillter)
         {
            case StatusFillter.AllStatus:
               listRaw = EFProvider.Instance.Database.Authors.ToList();
               break;

            case StatusFillter.Active:
               listRaw = EFProvider.Instance.Database.Authors.Where(x => x.Status == true).ToList();
               break;

            case StatusFillter.InActive:
               listRaw = EFProvider.Instance.Database.Authors.Where(x => x.Status == false).ToList();
               break;

            default:
               break;
         }

         foreach (var author in listRaw)
         {
            listAuthorDTO.Add(new AuthorDTO(author));
         }

         return listAuthorDTO;
      }

      public void Add(AuthorDTO newAuthor)
      {
         var newAu = newAuthor.GetBaseModel();
         EFProvider.Instance.SaveEntity(newAu, EntityState.Added, true);
      }

      public void Update(AuthorDTO author)
      {
         var authorUpdate = EFProvider.Instance.Database.Authors.Where(x => x.Id == author.Id).SingleOrDefault();
         if (authorUpdate != null)
         {
            authorUpdate.NickName = author.NickName;
            EFProvider.Instance.SaveEntity(authorUpdate, EntityState.Modified, true);
         }
      }

      public void ChangeStatus(int idAuthor)
      {
         var authorUpdate = EFProvider.Instance.Database.Authors.Where(x => x.Id == idAuthor).SingleOrDefault();

         if (authorUpdate != null)
         {
            authorUpdate.Status = authorUpdate.Status != true;
            EFProvider.Instance.SaveEntity(authorUpdate, EntityState.Modified, true);
         }
      }

      public void Delete(int idAuthor)
      {
         var authorDelete = EFProvider.Instance.Database.Authors.Where(x => x.Id == idAuthor).SingleOrDefault();

         if (authorDelete != null)
         {
            EFProvider.Instance.Database.Authors.Remove(authorDelete);
            EFProvider.Instance.Database.SaveChanges();
         }
      }

      private static AuthorDAL instance;
   }
}