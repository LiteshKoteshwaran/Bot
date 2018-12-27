using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicStoreMultiDialog
{
    [Serializable]
    public class DAL
    {
        static string Query;
        SQLOp sqlOp = new SQLOp();
        List<string> list = new List<string>(); 


        internal List<string> ShowAvailable(string selection)
        {
            Query = "select distinct Name from "+ selection ;
            list = sqlOp.GetListOnSelection(Query);
            return list;
        }
        internal List<string> ShowBrandOnSelection(string selection)
        {
            Query = "select distinct Brand.Name from Brand join Product on Product.BrandId = Brand.Id join SubCategory on Product.SubCategoryId = SubCategory.SubCategoryId join Category on Product.CategoryId = Category.Id where (SubCategory.SubCategoryName = '" + selection+ "' or Category.Name = '" + selection + "' )";
            list = sqlOp.GetListOnSelection(Query);
            return list;
        }
        internal List<string> ShowAvailableProductsOnSelection(string selection)
        {
            Query = "select distinct Product.Name from Product join Brand on Brand.Id = Product.BrandId join category on category.Id = Product.categoryid  join SubCategory on Product.SubCategoryId = SubCategory.SubCategoryId where(Brand.Name = '" + selection + "' or SubCategory.subcategoryName = '" + selection + "'or Category.name = '" + selection + "')";  
            list = sqlOp.GetListOnSelection(Query);
            return list;
        }

        internal List<string> ShowAvailableProductsOnSelectionOfCategoryAndBrand(string Category, string Brand)
        {
            Query = "select distinct Product.Name from Product join Brand on Brand.Id = Product.BrandId join category on category.Id = Product.categoryid  join SubCategory on Product.SubCategoryId = SubCategory.SubCategoryId where(Brand.Name = '" + Brand + "' and (SubCategory.subcategoryName = '" + Category + "'or Category.name = '" + Category + "'))";
            list = sqlOp.GetListOnSelection(Query);
            return list;
        }

        internal string GetProductDetail(string ProductName, string RequiredDetail)
        {
            Query = "select distinct " + RequiredDetail + " from Product where Name = '" + ProductName + "'";
            string Detail = sqlOp.GetSelection(Query);
            return Detail;
        }

        //internal List<string> GetDetail(string ProductName, string RequiredDetail)
        //{
        //    Query = "select distinct " + RequiredDetail + " from Product where Name = '" + ProductName + "'";
        //    list = sqlOp.GetListOnSelection(Query);
        //    return list;
        //}
        //internal List<string> Get(string ProductName, string RequiredDetail)
        //{
        //    Query = "select distinct " + RequiredDetail + " from Product where Name = '" + ProductName + "'";
        //    list = sqlOp.GetListOnSelection(Query);
        //    return list;
        //}



    }
}