namespace HaberSistemi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Olustur1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Haber",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Baslik = c.String(nullable: false, maxLength: 255),
                        KisaAciklama = c.String(),
                        Aciklama = c.String(),
                        Okunma = c.Int(nullable: false),
                        AktifMi = c.Boolean(nullable: false),
                        EklemeTarihi = c.DateTime(nullable: false),
                        Resim = c.String(nullable: false, maxLength: 255),
                        Kategori_ID = c.Int(),
                        Kullanici_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Kategori", t => t.Kategori_ID)
                .ForeignKey("dbo.Kullanici", t => t.Kullanici_ID)
                .Index(t => t.Kategori_ID)
                .Index(t => t.Kullanici_ID);
            
            CreateTable(
                "dbo.Kategori",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KategoriAdi = c.String(nullable: false, maxLength: 150),
                        ParentID = c.Int(nullable: false),
                        Url = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Resim",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ResimUrl = c.String(),
                        Haber_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Haber", t => t.Haber_ID)
                .Index(t => t.Haber_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resim", "Haber_ID", "dbo.Haber");
            DropForeignKey("dbo.Haber", "Kullanici_ID", "dbo.Kullanici");
            DropForeignKey("dbo.Haber", "Kategori_ID", "dbo.Kategori");
            DropIndex("dbo.Resim", new[] { "Haber_ID" });
            DropIndex("dbo.Haber", new[] { "Kullanici_ID" });
            DropIndex("dbo.Haber", new[] { "Kategori_ID" });
            DropTable("dbo.Resim");
            DropTable("dbo.Kategori");
            DropTable("dbo.Haber");
        }
    }
}
