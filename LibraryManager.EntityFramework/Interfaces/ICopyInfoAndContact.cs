namespace LibraryManager.EntityFramework
{
   public interface ICopyInfoAndContact : ICopyInfo
   {
      /// <summary>
      /// Copy Phone nummber đưa vào Clipboard
      /// </summary>
      System.Windows.Input.ICommand CopyPhoneNumberCommand { get; set; }

      /// <summary>
      /// Copy Address đưa vào Clipboard
      /// </summary>
      System.Windows.Input.ICommand CopyAddressCommand { get; set; }
   }
}