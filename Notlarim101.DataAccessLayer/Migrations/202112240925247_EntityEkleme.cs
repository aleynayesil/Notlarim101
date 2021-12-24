namespace Notlarim101.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityEkleme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblCategories", "Selamlama", c => c.String());
            AlterStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        Title = p.String(maxLength: 50),
                        Description = p.String(maxLength: 150),
                        Selamlama = p.String(),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                    },
                body:
                    @"INSERT [dbo].[tblCategories]([Title], [Description], [Selamlama], [CreatedOn], [ModifiedOn], [ModifiedUsername])
                      VALUES (@Title, @Description, @Selamlama, @CreatedOn, @ModifiedOn, @ModifiedUsername)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tblCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tblCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        Id = p.Int(),
                        Title = p.String(maxLength: 50),
                        Description = p.String(maxLength: 150),
                        Selamlama = p.String(),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                    },
                body:
                    @"UPDATE [dbo].[tblCategories]
                      SET [Title] = @Title, [Description] = @Description, [Selamlama] = @Selamlama, [CreatedOn] = @CreatedOn, [ModifiedOn] = @ModifiedOn, [ModifiedUsername] = @ModifiedUsername
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblCategories", "Selamlama");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
