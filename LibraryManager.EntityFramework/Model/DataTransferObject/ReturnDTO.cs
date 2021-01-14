namespace LibraryManager.EntityFramework.Model
{
   public class ReturnDTO : ReturnBook
   {
      public string LibrarianBorrow { get => (this.Borrow.Librarian.LastName + " " + this.Borrow.Librarian.FirstName).Trim(); } 
      public string LibrarianReturn { get => (this.Librarian.LastName + " " + this.Librarian.FirstName).Trim(); } 
      public string MemberName { get => (this.Borrow.Member.LastName + " " + this.Borrow.Member.FirstName).Trim(); } 

      public ReturnDTO() : base()
      {
      }

      public ReturnDTO(ReturnBook returnRaw) : base()
      {
         if (returnRaw != null)
         {
            this.Id = returnRaw.Id;
            this.BorrowId = returnRaw.BorrowId;
            this.ReturnDate = returnRaw.ReturnDate;
            this.LibrarianId = returnRaw.LibrarianId;

            this.Borrow = returnRaw.Borrow;
            this.Librarian = returnRaw.Librarian;
         }
      }
   }
}