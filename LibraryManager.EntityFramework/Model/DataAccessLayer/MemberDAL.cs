﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace LibraryManager.EntityFramework.Model
{
   /// <summary>
   /// Class Data Access Layer for Member
   /// </summary>
   public class MemberDAL : IDatabaseAccess<MemberDTO, string>
   {
      public static MemberDAL Instance
      {
         get
         {
            if (instance == null) { instance = new MemberDAL(); }
            return instance;
         }
      }

      private MemberDAL()
      {
      }

      public ObservableCollection<MemberDTO> GetList(EStatusFillter fillter = EStatusFillter.AllStatus)
      {
         var listMemberDTO = new ObservableCollection<MemberDTO>();
         var listRaw = new List<Member>();
         switch (fillter)
         {
            case EStatusFillter.AllStatus:
               listRaw = EFProvider.Instance.Database.Members.ToList();
               break;

            case EStatusFillter.Active:
               listRaw = EFProvider.Instance.Database.Members.Where(x => x.Status == true).ToList();
               break;

            case EStatusFillter.InActive:
               listRaw = EFProvider.Instance.Database.Members.Where(x => x.Status == false).ToList();
               break;
         }

         foreach (var mem in listRaw) { listMemberDTO.Add(new MemberDTO(mem)); }
         return listMemberDTO;
      }

      public ObservableCollection<MemberDTO> GetList(DateTime fromDate, DateTime toDate)
      {
         var listMemberDTO = new ObservableCollection<MemberDTO>();
         var listRaw = EFProvider.Instance.Database.Members.ToList();

         foreach (var item in listRaw)
         {
            if (item.RegisterDate.Value.Date >= fromDate.Date && item.RegisterDate.Value.Date <= toDate.Date)
            {
               listMemberDTO.Add(new MemberDTO(item));
            }
         }
         return listMemberDTO;
      }

      public void Add(MemberDTO newMember)
      {
         var newMem = newMember.GetBaseModel();
         EFProvider.Instance.SaveEntity(newMem, EntityState.Added);
      }

      public void Update(MemberDTO member)
      {
         var memberUpdate = EFProvider.Instance.Database.Members.Where(x => x.Id == member.Id).SingleOrDefault();
         if (memberUpdate != null)
         {
            memberUpdate.LastName = member.LastName;
            memberUpdate.FirstName = member.FirstName;
            memberUpdate.Sex = member.Sex;
            memberUpdate.Birthday = member.Birthday;
            memberUpdate.SSN = member.SSN;
            memberUpdate.Address = member.Address;
            memberUpdate.Email = member.Email;
            memberUpdate.PhoneNumber = member.PhoneNumber;

            memberUpdate.RegisterDate = member.RegisterDate;

            EFProvider.Instance.SaveEntity(memberUpdate, EntityState.Modified);
         }
      }

      public void ChangeStatus(string idMember)
      {
         var memberUpdate = EFProvider.Instance.Database.Members.Where(x => x.Id == idMember).SingleOrDefault();

         if (memberUpdate != null)
         {
            memberUpdate.Status = memberUpdate.Status != true;
            EFProvider.Instance.SaveEntity(memberUpdate, EntityState.Modified);
         }
      }

      public Member GetMember(string idMember)
      {
         return EFProvider.Instance.Database.Members.Where(x => x.Id == idMember).SingleOrDefault();
      }

      public void Delete(string objectId)
      {
         throw new NotImplementedException();
      }

      private static MemberDAL instance;
   }
}