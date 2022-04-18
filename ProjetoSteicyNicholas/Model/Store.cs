using System;
using Interfaces; 
using DAO;
using DTO;
using System.Collections.Generic;
namespace Model;

public class Store : IValidateDataObject, IDataController<StoreDTO, Store>
{
    private String name = "";
    private String CNPJ = "";
    private Owner owner;
    private Purchase purchase;
    public List<StoreDTO> StoreDTO = new List<StoreDTO>();
    List<Purchase> purchases = new List<Purchase>();
    public Store(Owner owner){this.owner=owner;}
    public void addNewPurchase(Purchase purchase){
        purchases.Add(purchase);
    }

    public Store(String name, String CNPJ, Owner owner, List<Purchase> purchase){
        this.name=name;
        this.CNPJ=CNPJ;
        this.owner=owner;
        this.purchase=purchase;
    }
    public static Store convertDTOToModel(StoreDTO obj)
    {
        return new Store(obj.name, obj.CNPJ, obj.owner, obj.purchase);
    }
    public void delete(AddressDTO obj)
    {

    }
     public int save()
    {
        var id = 0;

        using(var context = new DAOContext())
        {
            var store = new DAO.Store{
                name = this.name,
                CNPJ = this.CNPJ,
                owner = this.owner,
                purchase = this.purchase
            };

            context.Store.Add(store);

            context.SaveChanges();

            id = store.id;

        }
         return id;
    }

    public void update(StoreDTO obj)
    {

    }

    public StoreDTO findById(int id)
    {

        return new StoreDTO();
    }

    public List<StoreDTO> getAll()
    {        
        return this.storeDTO;      
    }

   
    public StoreDTO convertModelToDTO()
    {
        var storeDTO = new StoreDTO();

        StoreDTO.name = this.name;

        StoreDTO.CNPJ = this.CNPJ;

        StoreDTO.owner = this.owner;

        StoreDTO.purchase = this.purchase;

        return storeDTO;
    }

    public String getName(){
        return name;
    }
    public String getCNPJ(){
        return CNPJ;
    }
    public Owner getOwner(){
        return owner;
    }
    public Purchase getPurchase(){
        return purchase;
    }

    public void setName(String name){
        this.name=name;
    }
    public void setCNPJ(String CNPJ){
        this.CNPJ=CNPJ;
    }
    public void setOwner(Owner owner){
        this.owner=owner;
    }
    public void setPurchase(Purchase purchase){
        this.purchase=purchase;
    }
    public bool validateObject(Store obj)
    {
        if (obj.name == null)
            return false;
        else if (obj.CNPJ == null)
            return false;
        else if (obj.owner == null)
            return false;
        else if (obj.purchase == null)
            return false;
        else return true;

    }
}
