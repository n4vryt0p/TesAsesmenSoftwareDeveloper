using System.Security.Claims;
using AutoMapper;
using BackEnd.Database;
using BackEnd.Database.Models;
using DevExtreme.AspNet.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackEnd.Services;

public static class Masters
{
    public static WebApplication MasterData(this WebApplication app)
    {
        #region MapAPI

        _ = app.MapPost("/masterlistserverside", async (DataContext db) => await db.Products.IgnoreQueryFilters().AsNoTracking().ToListAsync())
            .WithTags("Masters")
            .WithName("GetMasterListServerside")
            .RequireAuthorization("admins_only");
            //.AllowAnonymous();

        _ = app.MapGet("/masterlist", async (DataContext db) => await db.Products.ToListAsync())
            .WithTags("Masters")
            .WithName("masterList")
            .RequireAuthorization("admins_only");
        //.AllowAnonymous();

        _ = app.MapGet("/masterlistddl", async (DataContext db) => await db.Products.Select(x => new GroupDdl
            {
                Id = x.Id, Text = x.Nama, ZoomId = x.HargaSatuan.ToString()

            }).ToListAsync())
            .WithTags("Masters")
            .WithName("masterListDdl")
            .RequireAuthorization("admins_only");

        //Add Master
        _ = app.MapPost("/master/add", async (Product masterdto, DataContext db) =>
            {
                _ = await db.Products.AddAsync(masterdto);
                _ = await db.SaveChangesAsync();
                return Results.Ok();
            })
            .WithTags("Masters")
            .WithName("AddMasters")
            .RequireAuthorization("admins_only");

        //Edit Master
        _ = app.MapPut("/master/edit/{masterId}", async (long masterId, MasterDto masterdto, DataContext db) =>
        {
            var masterX = await db.Products.FirstOrDefaultAsync(r => r.Id == masterId).ConfigureAwait(false);
            if (masterX == null)
                return Results.NotFound("No such master");
            masterX.Deskripsi = masterdto.Deskripsi ?? masterX.Deskripsi;
            masterX.Nama = masterdto.Nama ?? masterX.Nama;
            masterX.IsActive = masterdto.IsActive ?? masterX.IsActive;
            masterX.TanggalKadaluarsa = masterdto.TanggalKadaluarsa ?? masterX.TanggalKadaluarsa;
            masterX.Jumlah = masterdto.Jumlah > 0 ? masterdto.Jumlah : masterX.Jumlah;
            masterX.HargaSatuan = masterdto.HargaSatuan > 0 ? masterdto.HargaSatuan : masterX.HargaSatuan;

            _ = db.Products.Update(masterX);
            _ = await db.SaveChangesAsync();

            return Results.Ok();
        })
            .WithTags("Masters")
            .WithName("UpdateMaster")
            .RequireAuthorization("admins_only");

        //Delete Master
        _ = app.MapDelete("/master/delete/{masterId}",
                async (int masterId, DataContext db) =>
                {
                    var masterX = await db.Products.FirstOrDefaultAsync(r => r.Id == masterId).ConfigureAwait(false);
                    if (masterX == null)
                        return Results.NotFound("No such master");

                    _ = db.Products.Remove(masterX);
                    _ = await db.SaveChangesAsync();

                    return Results.Ok();
                })
            .WithTags("Masters")
            .WithName("DeleteMaster")
            .RequireAuthorization("admins_only");

        ////Import Master
        //_ = app.MapPost("/master/import",
        //        async (IFormFile file, DataContext db) =>
        //        {


        //            return Results.Ok();
        //        })
        //    .WithTags("Masters")
        //    .WithName("ImportMaster")
        //    .RequireAuthorization("admins_only");

        //News Set
        _ = app.MapPost("/transactionlistserverside", async (GridServerSide set, DataContext db, ClaimsPrincipal cp) =>
                {
                    IQueryable<TransactionDto>? newsDatas = null;
                    if (cp.IsInRole("SuperAdmin"))
                    {
                        newsDatas = db.Transactions.IgnoreQueryFilters().AsNoTracking().Select(yy => new TransactionDto
                        {
                            Id = yy.Id,
                            AlamatTujuan = yy.AlamatTujuan,
                            TotalHarga = yy.TotalHarga,
                            Tanggal = EF.Property<DateTime>(yy, "CreateDate")
                        });

                    }
                    else
                    {
                        var filter = cp.FindFirstValue("UserIds");
                        if (!string.IsNullOrEmpty(filter))
                        {
                            var filterInt = Convert.ToInt32(filter);
                            newsDatas = db.Transactions.IgnoreQueryFilters().AsNoTracking().Where(tt => tt.AppUserId == filterInt).Select(yy => new TransactionDto
                            {
                                Id = yy.Id,
                                AlamatTujuan = yy.AlamatTujuan,
                                TotalHarga = yy.TotalHarga,
                                Tanggal = EF.Property<DateTime>(yy, "CreateDate")
                            });
                        }
                    }

                    if (newsDatas == null) return new TransactionDtoResp(0, null);

                    var configs = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<GridServerSide, DataSourceLoadOptionsBase>().ForMember(dest => dest.Sort, act => act.MapFrom(src => src.Sorting));
                    });
                    var mappers = new Mapper(configs);
                    var settt = new DataSourceLoadOptionsBase();
                    mappers.Map(set, settt);
                    var obj = new List<TransactionDto>();
                    var ress = await DataSourceLoader.LoadAsync(newsDatas, settt);
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<TransactionDto, TransactionDto>();
                    });
                    var mapper = new Mapper(config);
                    mapper.Map(ress.data, obj);

                    return new TransactionDtoResp(ress.totalCount, obj);
                })
            .WithTags("Transaction")
            .WithName("TransactionListServerSide")
            .RequireAuthorization();

        //News Set
        _ = app.MapPost("/transactionlistchildserverside/{trxId}", async (long trxId, GridServerSide set, DataContext db, ClaimsPrincipal cp) =>
        {
            IQueryable<TransactionDto>? newsDatas = null;
            if (cp.IsInRole("SuperAdmin"))
            {
                newsDatas = db.ProductTansactions.IgnoreQueryFilters().AsNoTracking().Where(tr => tr.TransactionId == trxId).Select(yy => new TransactionDto
                {
                    ProductId = yy.ProductId,
                    TransactionId = yy.TransactionId,
                    NamaProduk = yy.Product!.Nama,
                    Kuantitas = yy.Kuantitas,
                    HargaSatuan = yy.HargaSatuanTrx
                });
            }
            else
            {
                var filter = cp.FindFirstValue("UserIds");
                if (!string.IsNullOrEmpty(filter))
                {
                    var filterInt = Convert.ToInt32(filter);
                    newsDatas = db.ProductTansactions.IgnoreQueryFilters().AsNoTracking().Where(tt => tt.TransactionId == trxId && tt.Transaction.AppUserId == filterInt).Select(yy => new TransactionDto
                    {
                        ProductId = yy.ProductId,
                        TransactionId = yy.TransactionId,
                        NamaProduk = yy.Product!.Nama,
                        Kuantitas = yy.Kuantitas,
                        HargaSatuan = yy.HargaSatuanTrx
                    });
                }
            }

            if (newsDatas == null) return new TransactionDtoResp(0, null);

            var configs = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GridServerSide, DataSourceLoadOptionsBase>().ForMember(dest => dest.Sort, act => act.MapFrom(src => src.Sorting));
            });
            var mappers = new Mapper(configs);
            var settt = new DataSourceLoadOptionsBase();
            mappers.Map(set, settt);
            var obj = new List<TransactionDto>();
            var ress = await DataSourceLoader.LoadAsync(newsDatas, settt);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TransactionDto, TransactionDto>();
            });
            var mapper = new Mapper(config);
            mapper.Map(ress.data, obj);

            return new TransactionDtoResp(ress.totalCount, obj);
        })
            .WithTags("Transaction")
            .WithName("TransactionListChildServerSide")
            .RequireAuthorization();

        //News Edit
        _ = app.MapPut("/transaction/edit/{trxId}/{prodId}", async (long trxId, int prodId, string newsDto, DataContext db, ClaimsPrincipal user) =>
            {
                var newsX = await db.Transactions.FirstOrDefaultAsync(r => r.Id == trxId).ConfigureAwait(false);
                if (newsX == null)
                    return Results.NotFound("No such master");
                var aUserNews = await db.ProductTansactions.FirstOrDefaultAsync(r => r.TransactionId == trxId && r.ProductId == prodId).ConfigureAwait(false);
                if (aUserNews == null)
                    return Results.NotFound("No such master");

                JsonConvert.PopulateObject(newsDto, newsX);
                _ = db.Entry(newsX).Property("UpdateBy").CurrentValue = user.Identity?.Name;
                _ = db.Entry(newsX).Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                _ = db.Transactions.Update(newsX);

                JsonConvert.PopulateObject(newsDto, aUserNews);
                _ = db.Entry(aUserNews).Property("UpdateBy").CurrentValue = user.Identity?.Name;
                _ = db.Entry(aUserNews).Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                _ = db.ProductTansactions.Update(aUserNews);

                _ = await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithTags("Transaction")
            .WithName("UpdateTransaction")
            .RequireAuthorization();

        //Delete News
        _ = app.MapDelete("/transaction/delete/{trxId}/{prodId}",
                async (long trxId, int prodId, DataContext db) =>
                {
                    var newsX = await db.ProductTansactions.FirstOrDefaultAsync(r => r.ProductId == prodId && r.TransactionId == trxId).ConfigureAwait(false);
                    if (newsX == null)
                        return Results.NotFound("No such master");

                    _ = db.ProductTansactions.Remove(newsX);
                    _ = await db.SaveChangesAsync();

                    return Results.Ok();
                })
            .WithTags("Transaction")
            .WithName("DeleteTransaction")
            .RequireAuthorization();

        //News Edit
        _ = app.MapPut("/transaction/save/{trxId}", async (long trxId, string trxDto, DataContext db, ClaimsPrincipal user) =>
            {
                var newsX = await db.Transactions.FirstOrDefaultAsync(r => r.Id == trxId).ConfigureAwait(false);
                if (newsX == null)
                    return Results.NotFound("No such master");

                JsonConvert.PopulateObject(trxDto, newsX);
                _ = db.Entry(newsX).Property("UpdateBy").CurrentValue = user.Identity?.Name;
                _ = db.Entry(newsX).Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                _ = db.Transactions.Update(newsX);
                _ = await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithTags("Transaction")
            .WithName("SaveTransaction")
            .RequireAuthorization();

        //News Detail
        _ = app.MapPost("/Transactiondetails/{newsId}", async (long newsId, DataContext db) => await db.Transactions.AsNoTracking().Where(tt => tt.Id == newsId).Select(yy => yy.AlamatTujuan).FirstOrDefaultAsync())
            .WithTags("Transaction")
            .WithName("TransactionDetails")
            .RequireAuthorization();

        #endregion
        return app;
    }
}

internal record MasterDto(int Id, string? Nama, string? Deskripsi, DateTime? TanggalKadaluarsa
    , int Jumlah, int HargaSatuan, bool? IsActive);

internal record TransactionDtoResp(long TotalCount, IEnumerable<TransactionDto>? Data)
{
}
public class TransactionDto
{
    public long Id { get; set; }
    public int ProductId { get; set; }
    public long TransactionId { get; set; }
    public string? AlamatTujuan { get; set; }
    public int TotalHarga { get; set; }
    public DateTime? Tanggal { get; set; }
    public string? NamaProduk { get; set; }
    public int Kuantitas { get; set; }
    public int HargaSatuan { get; set; }
    //public ICollection<ProductDto> Products { get; } = new List<ProductDto>();
}
//public class ProductDto
//{
//    public string? Nama { get; set; }
//    public int Kuantitas { get; set; }
//    public int HargaSatuan { get; set; }
//    public int Jumlah { get; set; }
//}