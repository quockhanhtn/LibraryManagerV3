using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManager.EntityFramework.Model
{
   /// <summary>
   /// Class Data Access Layer for BookCategory
   /// </summary>
   public class BookCategoryDAL : IDatabaseAccess<BookCategoryDTO, int>
   {
      public static BookCategoryDAL Instance
      {
         get
         {
            if (instance == null) { instance = new BookCategoryDAL(); }
            return instance;
         }
      }

      private BookCategoryDAL()
      {
      }

      public ObservableCollection<BookCategoryDTO> GetList(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         var listRaw = new List<BookCategory>();
         var listBookCategoryDTO = new ObservableCollection<BookCategoryDTO>();

         switch (fillter)
         {
            case EStatusFillter.AllStatus:
               listRaw = EFProvider.Instance.Database.BookCategories.ToList();
               break;

            case EStatusFillter.Active:
               listRaw = EFProvider.Instance.Database.BookCategories.Where(x => x.Status == true).ToList();
               break;

            case EStatusFillter.InActive:
               listRaw = EFProvider.Instance.Database.BookCategories.Where(x => x.Status == false).ToList();
               break;
         }

         foreach (var bookCategory in listRaw) { listBookCategoryDTO.Add(new BookCategoryDTO(bookCategory)); }
         return listBookCategoryDTO;
      }

      public void Add(BookCategoryDTO newBookCategory)
      {
         var newBC = newBookCategory.GetBaseModel();
         EFProvider.Instance.SaveEntity(newBC, EntityState.Added, true);
      }

      public void Update(BookCategoryDTO bookCategory)
      {
         var bookCategoryUpdate = EFProvider.Instance.Database.BookCategories.Where(x => x.Id == bookCategory.Id).SingleOrDefault();
         if (bookCategoryUpdate != null)
         {
            bookCategoryUpdate.Name = bookCategory.Name;
            bookCategoryUpdate.LimitDays = bookCategory.LimitDays;

            EFProvider.Instance.SaveEntity(bookCategoryUpdate, EntityState.Modified, true);
         }
      }

      public void ChangeStatus(int idBookCategory)
      {
         var bookCategoryUpdate = EFProvider.Instance.Database.BookCategories.Where(x => x.Id == idBookCategory).SingleOrDefault();

         if (bookCategoryUpdate != null)
         {
            bookCategoryUpdate.Status = bookCategoryUpdate.Status != true;
            EFProvider.Instance.SaveEntity(bookCategoryUpdate, EntityState.Modified, true);
         }
      }

      public void Delete(int idBookCategory)
      {
         var bookCategoryDelete = EFProvider.Instance.Database.BookCategories.Where(x => x.Id == idBookCategory).SingleOrDefault();

         if (bookCategoryDelete != null)
         {
            EFProvider.Instance.Database.BookCategories.Remove(bookCategoryDelete);
            EFProvider.Instance.Database.SaveChanges();
         }
      }

      private static BookCategoryDAL instance;
   }
}