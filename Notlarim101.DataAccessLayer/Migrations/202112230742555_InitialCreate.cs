namespace Notlarim101.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 150),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        Text = c.String(nullable: false, maxLength: 2000),
                        IsDraft = c.Boolean(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.tblNotlarimUsers", t => t.Owner_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.tblComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 300),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                        Note_Id = c.Int(),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblNotes", t => t.Note_Id)
                .ForeignKey("dbo.tblNotlarimUsers", t => t.Owner_Id)
                .Index(t => t.Note_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.tblNotlarimUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Surname = c.String(maxLength: 30),
                        Username = c.String(nullable: false, maxLength: 30),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        ProfileImageFilename = c.String(maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                        ActivateGuid = c.Guid(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblLiked",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LikedUser_Id = c.Int(),
                        Note_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblNotlarimUsers", t => t.LikedUser_Id)
                .ForeignKey("dbo.tblNotes", t => t.Note_Id)
                .Index(t => t.LikedUser_Id)
                .Index(t => t.Note_Id);
            
            CreateStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        Title = p.String(maxLength: 50),
                        Description = p.String(maxLength: 150),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                    },
                body:
                    @"INSERT [dbo].[tblCategories]([Title], [Description], [CreatedOn], [ModifiedOn], [ModifiedUsername])
                      VALUES (@Title, @Description, @CreatedOn, @ModifiedOn, @ModifiedUsername)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tblCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tblCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        Id = p.Int(),
                        Title = p.String(maxLength: 50),
                        Description = p.String(maxLength: 150),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                    },
                body:
                    @"UPDATE [dbo].[tblCategories]
                      SET [Title] = @Title, [Description] = @Description, [CreatedOn] = @CreatedOn, [ModifiedOn] = @ModifiedOn, [ModifiedUsername] = @ModifiedUsername
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tblCategories]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Note_Insert",
                p => new
                    {
                        Title = p.String(maxLength: 60),
                        Text = p.String(maxLength: 2000),
                        IsDraft = p.Boolean(),
                        LikeCount = p.Int(),
                        CategoryId = p.Int(),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                        Owner_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tblNotes]([Title], [Text], [IsDraft], [LikeCount], [CategoryId], [CreatedOn], [ModifiedOn], [ModifiedUsername], [Owner_Id])
                      VALUES (@Title, @Text, @IsDraft, @LikeCount, @CategoryId, @CreatedOn, @ModifiedOn, @ModifiedUsername, @Owner_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tblNotes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tblNotes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Note_Update",
                p => new
                    {
                        Id = p.Int(),
                        Title = p.String(maxLength: 60),
                        Text = p.String(maxLength: 2000),
                        IsDraft = p.Boolean(),
                        LikeCount = p.Int(),
                        CategoryId = p.Int(),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                        Owner_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tblNotes]
                      SET [Title] = @Title, [Text] = @Text, [IsDraft] = @IsDraft, [LikeCount] = @LikeCount, [CategoryId] = @CategoryId, [CreatedOn] = @CreatedOn, [ModifiedOn] = @ModifiedOn, [ModifiedUsername] = @ModifiedUsername, [Owner_Id] = @Owner_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Note_Delete",
                p => new
                    {
                        Id = p.Int(),
                        Owner_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tblNotes]
                      WHERE (([Id] = @Id) AND (([Owner_Id] = @Owner_Id) OR ([Owner_Id] IS NULL AND @Owner_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.Comment_Insert",
                p => new
                    {
                        Text = p.String(maxLength: 300),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                        Note_Id = p.Int(),
                        Owner_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tblComments]([Text], [CreatedOn], [ModifiedOn], [ModifiedUsername], [Note_Id], [Owner_Id])
                      VALUES (@Text, @CreatedOn, @ModifiedOn, @ModifiedUsername, @Note_Id, @Owner_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tblComments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tblComments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Comment_Update",
                p => new
                    {
                        Id = p.Int(),
                        Text = p.String(maxLength: 300),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                        Note_Id = p.Int(),
                        Owner_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tblComments]
                      SET [Text] = @Text, [CreatedOn] = @CreatedOn, [ModifiedOn] = @ModifiedOn, [ModifiedUsername] = @ModifiedUsername, [Note_Id] = @Note_Id, [Owner_Id] = @Owner_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Comment_Delete",
                p => new
                    {
                        Id = p.Int(),
                        Note_Id = p.Int(),
                        Owner_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tblComments]
                      WHERE ((([Id] = @Id) AND (([Note_Id] = @Note_Id) OR ([Note_Id] IS NULL AND @Note_Id IS NULL))) AND (([Owner_Id] = @Owner_Id) OR ([Owner_Id] IS NULL AND @Owner_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.NotlarimUser_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 30),
                        Surname = p.String(maxLength: 30),
                        Username = p.String(maxLength: 30),
                        Email = p.String(maxLength: 100),
                        Password = p.String(maxLength: 100),
                        ProfileImageFilename = p.String(maxLength: 30),
                        IsActive = p.Boolean(),
                        ActivateGuid = p.Guid(),
                        IsAdmin = p.Boolean(),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                    },
                body:
                    @"INSERT [dbo].[tblNotlarimUsers]([Name], [Surname], [Username], [Email], [Password], [ProfileImageFilename], [IsActive], [ActivateGuid], [IsAdmin], [CreatedOn], [ModifiedOn], [ModifiedUsername])
                      VALUES (@Name, @Surname, @Username, @Email, @Password, @ProfileImageFilename, @IsActive, @ActivateGuid, @IsAdmin, @CreatedOn, @ModifiedOn, @ModifiedUsername)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tblNotlarimUsers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tblNotlarimUsers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.NotlarimUser_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 30),
                        Surname = p.String(maxLength: 30),
                        Username = p.String(maxLength: 30),
                        Email = p.String(maxLength: 100),
                        Password = p.String(maxLength: 100),
                        ProfileImageFilename = p.String(maxLength: 30),
                        IsActive = p.Boolean(),
                        ActivateGuid = p.Guid(),
                        IsAdmin = p.Boolean(),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        ModifiedUsername = p.String(maxLength: 30),
                    },
                body:
                    @"UPDATE [dbo].[tblNotlarimUsers]
                      SET [Name] = @Name, [Surname] = @Surname, [Username] = @Username, [Email] = @Email, [Password] = @Password, [ProfileImageFilename] = @ProfileImageFilename, [IsActive] = @IsActive, [ActivateGuid] = @ActivateGuid, [IsAdmin] = @IsAdmin, [CreatedOn] = @CreatedOn, [ModifiedOn] = @ModifiedOn, [ModifiedUsername] = @ModifiedUsername
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotlarimUser_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tblNotlarimUsers]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Liked_Insert",
                p => new
                    {
                        LikedUser_Id = p.Int(),
                        Note_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tblLiked]([LikedUser_Id], [Note_Id])
                      VALUES (@LikedUser_Id, @Note_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tblLiked]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tblLiked] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Liked_Update",
                p => new
                    {
                        Id = p.Int(),
                        LikedUser_Id = p.Int(),
                        Note_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tblLiked]
                      SET [LikedUser_Id] = @LikedUser_Id, [Note_Id] = @Note_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Liked_Delete",
                p => new
                    {
                        Id = p.Int(),
                        LikedUser_Id = p.Int(),
                        Note_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tblLiked]
                      WHERE ((([Id] = @Id) AND (([LikedUser_Id] = @LikedUser_Id) OR ([LikedUser_Id] IS NULL AND @LikedUser_Id IS NULL))) AND (([Note_Id] = @Note_Id) OR ([Note_Id] IS NULL AND @Note_Id IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Liked_Delete");
            DropStoredProcedure("dbo.Liked_Update");
            DropStoredProcedure("dbo.Liked_Insert");
            DropStoredProcedure("dbo.NotlarimUser_Delete");
            DropStoredProcedure("dbo.NotlarimUser_Update");
            DropStoredProcedure("dbo.NotlarimUser_Insert");
            DropStoredProcedure("dbo.Comment_Delete");
            DropStoredProcedure("dbo.Comment_Update");
            DropStoredProcedure("dbo.Comment_Insert");
            DropStoredProcedure("dbo.Note_Delete");
            DropStoredProcedure("dbo.Note_Update");
            DropStoredProcedure("dbo.Note_Insert");
            DropStoredProcedure("dbo.Category_Delete");
            DropStoredProcedure("dbo.Category_Update");
            DropStoredProcedure("dbo.Category_Insert");
            DropForeignKey("dbo.tblNotes", "Owner_Id", "dbo.tblNotlarimUsers");
            DropForeignKey("dbo.tblLiked", "Note_Id", "dbo.tblNotes");
            DropForeignKey("dbo.tblLiked", "LikedUser_Id", "dbo.tblNotlarimUsers");
            DropForeignKey("dbo.tblComments", "Owner_Id", "dbo.tblNotlarimUsers");
            DropForeignKey("dbo.tblComments", "Note_Id", "dbo.tblNotes");
            DropForeignKey("dbo.tblNotes", "CategoryId", "dbo.tblCategories");
            DropIndex("dbo.tblLiked", new[] { "Note_Id" });
            DropIndex("dbo.tblLiked", new[] { "LikedUser_Id" });
            DropIndex("dbo.tblComments", new[] { "Owner_Id" });
            DropIndex("dbo.tblComments", new[] { "Note_Id" });
            DropIndex("dbo.tblNotes", new[] { "Owner_Id" });
            DropIndex("dbo.tblNotes", new[] { "CategoryId" });
            DropTable("dbo.tblLiked");
            DropTable("dbo.tblNotlarimUsers");
            DropTable("dbo.tblComments");
            DropTable("dbo.tblNotes");
            DropTable("dbo.tblCategories");
        }
    }
}
