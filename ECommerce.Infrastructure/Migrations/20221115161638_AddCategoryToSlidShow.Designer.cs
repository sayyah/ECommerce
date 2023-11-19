﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.DataContext.Migrations
{
    [DbContext(typeof(SunflowerECommerceDbContext))]
    [Migration("20221115161638_AddCategoryToSlidShow")]
    partial class AddCategoryToSlidShow
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlogKeyword", b =>
                {
                    b.Property<int>("BlogsId")
                        .HasColumnType("int");

                    b.Property<int>("KeywordsId")
                        .HasColumnType("int");

                    b.HasKey("BlogsId", "KeywordsId");

                    b.HasIndex("KeywordsId");

                    b.ToTable("BlogKeyword");
                });

            modelBuilder.Entity("BlogTag", b =>
                {
                    b.Property<int>("BlogsId")
                        .HasColumnType("int");

                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.HasKey("BlogsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("BlogTag");
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<int>("ProductCategoriesId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("ProductCategoriesId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("Entities.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BlogAuthorId")
                        .HasColumnType("int");

                    b.Property<int>("BlogCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Dislike")
                        .HasColumnType("int");

                    b.Property<DateTime>("EditDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Like")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Visit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlogAuthorId");

                    b.HasIndex("BlogCategoryId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("Entities.BlogAuthor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("BlogAuthors");
                });

            modelBuilder.Entity("Entities.BlogCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("Depth")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("BlogCategories");
                });

            modelBuilder.Entity("Entities.BlogComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AnswerId")
                        .HasColumnType("int");

                    b.Property<int?>("BlogId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAnswered")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("BlogId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("UserId");

                    b.ToTable("BlogComments");
                });

            modelBuilder.Entity("Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Depth")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("StateId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "اهر",
                            StateId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "عجبشير",
                            StateId = 1
                        },
                        new
                        {
                            Id = 3,
                            Name = "آذر شهر",
                            StateId = 1
                        },
                        new
                        {
                            Id = 4,
                            Name = "بناب",
                            StateId = 1
                        },
                        new
                        {
                            Id = 5,
                            Name = "بستان آباد",
                            StateId = 1
                        },
                        new
                        {
                            Id = 6,
                            Name = "چاراويماق",
                            StateId = 1
                        },
                        new
                        {
                            Id = 7,
                            Name = "هشترود",
                            StateId = 1
                        },
                        new
                        {
                            Id = 8,
                            Name = "هريس",
                            StateId = 1
                        },
                        new
                        {
                            Id = 9,
                            Name = "جلفا",
                            StateId = 1
                        },
                        new
                        {
                            Id = 10,
                            Name = "كليبر",
                            StateId = 1
                        },
                        new
                        {
                            Id = 11,
                            Name = "خداآفرين",
                            StateId = 1
                        },
                        new
                        {
                            Id = 12,
                            Name = "ملكان",
                            StateId = 1
                        },
                        new
                        {
                            Id = 13,
                            Name = "مراغه",
                            StateId = 1
                        },
                        new
                        {
                            Id = 14,
                            Name = "ميانه",
                            StateId = 1
                        },
                        new
                        {
                            Id = 15,
                            Name = "مرند",
                            StateId = 1
                        },
                        new
                        {
                            Id = 16,
                            Name = "اسكو",
                            StateId = 1
                        },
                        new
                        {
                            Id = 17,
                            Name = "سراب",
                            StateId = 1
                        },
                        new
                        {
                            Id = 18,
                            Name = "شبستر",
                            StateId = 1
                        },
                        new
                        {
                            Id = 19,
                            Name = "تبريز",
                            StateId = 1
                        },
                        new
                        {
                            Id = 20,
                            Name = "ورزقان",
                            StateId = 1
                        },
                        new
                        {
                            Id = 21,
                            Name = "اروميه",
                            StateId = 2
                        },
                        new
                        {
                            Id = 22,
                            Name = "نقده",
                            StateId = 2
                        },
                        new
                        {
                            Id = 23,
                            Name = "ماكو",
                            StateId = 2
                        },
                        new
                        {
                            Id = 24,
                            Name = "تكاب",
                            StateId = 2
                        },
                        new
                        {
                            Id = 25,
                            Name = "خوي",
                            StateId = 2
                        },
                        new
                        {
                            Id = 26,
                            Name = "مهاباد",
                            StateId = 2
                        },
                        new
                        {
                            Id = 27,
                            Name = "سر دشت",
                            StateId = 2
                        },
                        new
                        {
                            Id = 28,
                            Name = "چالدران",
                            StateId = 2
                        },
                        new
                        {
                            Id = 29,
                            Name = "بوكان",
                            StateId = 2
                        },
                        new
                        {
                            Id = 30,
                            Name = "مياندوآب",
                            StateId = 2
                        },
                        new
                        {
                            Id = 31,
                            Name = "سلماس",
                            StateId = 2
                        },
                        new
                        {
                            Id = 32,
                            Name = "شاهين دژ",
                            StateId = 2
                        },
                        new
                        {
                            Id = 33,
                            Name = "پيرانشهر",
                            StateId = 2
                        },
                        new
                        {
                            Id = 34,
                            Name = "اشنويه",
                            StateId = 2
                        },
                        new
                        {
                            Id = 35,
                            Name = "چايپاره",
                            StateId = 2
                        },
                        new
                        {
                            Id = 36,
                            Name = "پلدشت",
                            StateId = 2
                        },
                        new
                        {
                            Id = 37,
                            Name = "شوط",
                            StateId = 2
                        },
                        new
                        {
                            Id = 38,
                            Name = "اردبيل",
                            StateId = 3
                        },
                        new
                        {
                            Id = 39,
                            Name = "سرعين",
                            StateId = 3
                        },
                        new
                        {
                            Id = 40,
                            Name = "بيله سوار",
                            StateId = 3
                        },
                        new
                        {
                            Id = 41,
                            Name = "پارس آباد",
                            StateId = 3
                        },
                        new
                        {
                            Id = 42,
                            Name = "خلخال",
                            StateId = 3
                        },
                        new
                        {
                            Id = 43,
                            Name = "مشگين شهر",
                            StateId = 3
                        },
                        new
                        {
                            Id = 44,
                            Name = "نمين",
                            StateId = 3
                        },
                        new
                        {
                            Id = 45,
                            Name = "نير",
                            StateId = 3
                        },
                        new
                        {
                            Id = 46,
                            Name = "كوثر",
                            StateId = 3
                        },
                        new
                        {
                            Id = 47,
                            Name = "گرمي",
                            StateId = 3
                        },
                        new
                        {
                            Id = 48,
                            Name = "بوئين و مياندشت",
                            StateId = 4
                        },
                        new
                        {
                            Id = 49,
                            Name = "مباركه",
                            StateId = 4
                        },
                        new
                        {
                            Id = 50,
                            Name = "اردستان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 51,
                            Name = "خور و بيابانک",
                            StateId = 4
                        },
                        new
                        {
                            Id = 52,
                            Name = "فلاورجان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 53,
                            Name = "فريدون شهر",
                            StateId = 4
                        },
                        new
                        {
                            Id = 54,
                            Name = "كاشان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 55,
                            Name = "لنجان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 56,
                            Name = "گلپايگان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 57,
                            Name = "فريدن",
                            StateId = 4
                        },
                        new
                        {
                            Id = 58,
                            Name = "نايين",
                            StateId = 4
                        },
                        new
                        {
                            Id = 59,
                            Name = "اصفهان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 60,
                            Name = "نجف آباد",
                            StateId = 4
                        },
                        new
                        {
                            Id = 61,
                            Name = "آران و بيدگل",
                            StateId = 4
                        },
                        new
                        {
                            Id = 62,
                            Name = "چادگان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 63,
                            Name = "تيران و کرون",
                            StateId = 4
                        },
                        new
                        {
                            Id = 64,
                            Name = "شهرضا",
                            StateId = 4
                        },
                        new
                        {
                            Id = 65,
                            Name = "سميرم",
                            StateId = 4
                        },
                        new
                        {
                            Id = 66,
                            Name = "خميني شهر",
                            StateId = 4
                        },
                        new
                        {
                            Id = 67,
                            Name = "دهاقان",
                            StateId = 4
                        },
                        new
                        {
                            Id = 68,
                            Name = "نطنز",
                            StateId = 4
                        },
                        new
                        {
                            Id = 69,
                            Name = "برخوار",
                            StateId = 4
                        },
                        new
                        {
                            Id = 70,
                            Name = "شاهين شهر و ميمه",
                            StateId = 4
                        },
                        new
                        {
                            Id = 71,
                            Name = "خوانسار",
                            StateId = 4
                        },
                        new
                        {
                            Id = 72,
                            Name = "ايلام",
                            StateId = 6
                        },
                        new
                        {
                            Id = 73,
                            Name = "مهران",
                            StateId = 6
                        },
                        new
                        {
                            Id = 74,
                            Name = "دهلران",
                            StateId = 6
                        },
                        new
                        {
                            Id = 75,
                            Name = "آبدانان",
                            StateId = 6
                        },
                        new
                        {
                            Id = 76,
                            Name = "چرداول",
                            StateId = 6
                        },
                        new
                        {
                            Id = 77,
                            Name = "دره شهر",
                            StateId = 6
                        },
                        new
                        {
                            Id = 78,
                            Name = "ايوان",
                            StateId = 6
                        },
                        new
                        {
                            Id = 79,
                            Name = "بدره",
                            StateId = 6
                        },
                        new
                        {
                            Id = 80,
                            Name = "سيروان",
                            StateId = 6
                        },
                        new
                        {
                            Id = 81,
                            Name = "ملکشاهي",
                            StateId = 6
                        },
                        new
                        {
                            Id = 82,
                            Name = "عسلويه",
                            StateId = 7
                        },
                        new
                        {
                            Id = 83,
                            Name = "گناوه",
                            StateId = 7
                        },
                        new
                        {
                            Id = 84,
                            Name = "دشتي",
                            StateId = 7
                        },
                        new
                        {
                            Id = 85,
                            Name = "دشتستان",
                            StateId = 7
                        },
                        new
                        {
                            Id = 86,
                            Name = "دير",
                            StateId = 7
                        },
                        new
                        {
                            Id = 87,
                            Name = "بوشهر",
                            StateId = 7
                        },
                        new
                        {
                            Id = 88,
                            Name = "كنگان",
                            StateId = 7
                        },
                        new
                        {
                            Id = 89,
                            Name = "تنگستان",
                            StateId = 7
                        },
                        new
                        {
                            Id = 90,
                            Name = "ديلم",
                            StateId = 7
                        },
                        new
                        {
                            Id = 91,
                            Name = "جم",
                            StateId = 7
                        },
                        new
                        {
                            Id = 92,
                            Name = "قرچك",
                            StateId = 8
                        },
                        new
                        {
                            Id = 93,
                            Name = "پرديس",
                            StateId = 8
                        },
                        new
                        {
                            Id = 94,
                            Name = "بهارستان",
                            StateId = 8
                        },
                        new
                        {
                            Id = 95,
                            Name = "شميرانات",
                            StateId = 8
                        },
                        new
                        {
                            Id = 96,
                            Name = "رباط كريم",
                            StateId = 8
                        },
                        new
                        {
                            Id = 97,
                            Name = "فيروزكوه",
                            StateId = 8
                        },
                        new
                        {
                            Id = 98,
                            Name = "تهران",
                            StateId = 8
                        },
                        new
                        {
                            Id = 99,
                            Name = "ورامين",
                            StateId = 8
                        },
                        new
                        {
                            Id = 100,
                            Name = "اسلامشهر",
                            StateId = 8
                        },
                        new
                        {
                            Id = 101,
                            Name = "ري",
                            StateId = 8
                        },
                        new
                        {
                            Id = 102,
                            Name = "پاكدشت",
                            StateId = 8
                        },
                        new
                        {
                            Id = 103,
                            Name = "پيشوا",
                            StateId = 8
                        },
                        new
                        {
                            Id = 104,
                            Name = "قدس",
                            StateId = 8
                        },
                        new
                        {
                            Id = 105,
                            Name = "ملارد",
                            StateId = 8
                        },
                        new
                        {
                            Id = 106,
                            Name = "شهريار",
                            StateId = 8
                        },
                        new
                        {
                            Id = 107,
                            Name = "دماوند",
                            StateId = 8
                        },
                        new
                        {
                            Id = 108,
                            Name = "بن",
                            StateId = 9
                        },
                        new
                        {
                            Id = 109,
                            Name = "سامان",
                            StateId = 9
                        },
                        new
                        {
                            Id = 110,
                            Name = "کيار",
                            StateId = 9
                        },
                        new
                        {
                            Id = 111,
                            Name = "بروجن",
                            StateId = 9
                        },
                        new
                        {
                            Id = 112,
                            Name = "اردل",
                            StateId = 9
                        },
                        new
                        {
                            Id = 113,
                            Name = "شهركرد",
                            StateId = 9
                        },
                        new
                        {
                            Id = 114,
                            Name = "فارسان",
                            StateId = 9
                        },
                        new
                        {
                            Id = 115,
                            Name = "کوهرنگ",
                            StateId = 9
                        },
                        new
                        {
                            Id = 116,
                            Name = "لردگان",
                            StateId = 9
                        },
                        new
                        {
                            Id = 117,
                            Name = "داورزن",
                            StateId = 11
                        },
                        new
                        {
                            Id = 118,
                            Name = "كلات",
                            StateId = 11
                        },
                        new
                        {
                            Id = 119,
                            Name = "بردسكن",
                            StateId = 11
                        },
                        new
                        {
                            Id = 120,
                            Name = "مشهد",
                            StateId = 11
                        },
                        new
                        {
                            Id = 121,
                            Name = "نيشابور",
                            StateId = 11
                        },
                        new
                        {
                            Id = 122,
                            Name = "سبزوار",
                            StateId = 11
                        },
                        new
                        {
                            Id = 123,
                            Name = "كاشمر",
                            StateId = 11
                        },
                        new
                        {
                            Id = 124,
                            Name = "گناباد",
                            StateId = 11
                        },
                        new
                        {
                            Id = 125,
                            Name = "تربت حيدريه",
                            StateId = 11
                        },
                        new
                        {
                            Id = 126,
                            Name = "خواف",
                            StateId = 11
                        },
                        new
                        {
                            Id = 127,
                            Name = "تربت جام",
                            StateId = 11
                        },
                        new
                        {
                            Id = 128,
                            Name = "تايباد",
                            StateId = 11
                        },
                        new
                        {
                            Id = 129,
                            Name = "مه ولات",
                            StateId = 11
                        },
                        new
                        {
                            Id = 130,
                            Name = "چناران",
                            StateId = 11
                        },
                        new
                        {
                            Id = 131,
                            Name = "درگز",
                            StateId = 11
                        },
                        new
                        {
                            Id = 132,
                            Name = "فيروزه",
                            StateId = 11
                        },
                        new
                        {
                            Id = 133,
                            Name = "قوچان",
                            StateId = 11
                        },
                        new
                        {
                            Id = 134,
                            Name = "سرخس",
                            StateId = 11
                        },
                        new
                        {
                            Id = 135,
                            Name = "رشتخوار",
                            StateId = 11
                        },
                        new
                        {
                            Id = 136,
                            Name = "بينالود",
                            StateId = 11
                        },
                        new
                        {
                            Id = 137,
                            Name = "زاوه",
                            StateId = 11
                        },
                        new
                        {
                            Id = 138,
                            Name = "جوين",
                            StateId = 11
                        },
                        new
                        {
                            Id = 139,
                            Name = "بجستان",
                            StateId = 11
                        },
                        new
                        {
                            Id = 140,
                            Name = "باخزر",
                            StateId = 11
                        },
                        new
                        {
                            Id = 141,
                            Name = "فريمان",
                            StateId = 11
                        },
                        new
                        {
                            Id = 142,
                            Name = "خليل آباد",
                            StateId = 11
                        },
                        new
                        {
                            Id = 143,
                            Name = "جغتاي",
                            StateId = 11
                        },
                        new
                        {
                            Id = 144,
                            Name = "خوشاب",
                            StateId = 11
                        },
                        new
                        {
                            Id = 145,
                            Name = "زيرکوه",
                            StateId = 10
                        },
                        new
                        {
                            Id = 146,
                            Name = "خوسف",
                            StateId = 10
                        },
                        new
                        {
                            Id = 147,
                            Name = "درميان",
                            StateId = 10
                        },
                        new
                        {
                            Id = 148,
                            Name = "قائنات",
                            StateId = 10
                        },
                        new
                        {
                            Id = 149,
                            Name = "بشرويه",
                            StateId = 10
                        },
                        new
                        {
                            Id = 150,
                            Name = "فردوس",
                            StateId = 10
                        },
                        new
                        {
                            Id = 151,
                            Name = "بيرجند",
                            StateId = 10
                        },
                        new
                        {
                            Id = 152,
                            Name = "نهبندان",
                            StateId = 10
                        },
                        new
                        {
                            Id = 153,
                            Name = "سربيشه",
                            StateId = 10
                        },
                        new
                        {
                            Id = 154,
                            Name = "سرايان",
                            StateId = 10
                        },
                        new
                        {
                            Id = 155,
                            Name = "طبس",
                            StateId = 11
                        },
                        new
                        {
                            Id = 156,
                            Name = "بجنورد",
                            StateId = 12
                        },
                        new
                        {
                            Id = 157,
                            Name = "راز و جرگلان",
                            StateId = 12
                        },
                        new
                        {
                            Id = 158,
                            Name = "اسفراين",
                            StateId = 12
                        },
                        new
                        {
                            Id = 159,
                            Name = "جاجرم",
                            StateId = 12
                        },
                        new
                        {
                            Id = 160,
                            Name = "شيروان",
                            StateId = 12
                        },
                        new
                        {
                            Id = 161,
                            Name = "مانه و سملقان",
                            StateId = 12
                        },
                        new
                        {
                            Id = 162,
                            Name = "گرمه",
                            StateId = 12
                        },
                        new
                        {
                            Id = 163,
                            Name = "فاروج",
                            StateId = 12
                        },
                        new
                        {
                            Id = 164,
                            Name = "کارون",
                            StateId = 13
                        },
                        new
                        {
                            Id = 165,
                            Name = "حميديه",
                            StateId = 13
                        },
                        new
                        {
                            Id = 166,
                            Name = "آغاجري",
                            StateId = 13
                        },
                        new
                        {
                            Id = 167,
                            Name = "شوشتر",
                            StateId = 13
                        },
                        new
                        {
                            Id = 168,
                            Name = "دشت آزادگان",
                            StateId = 13
                        },
                        new
                        {
                            Id = 169,
                            Name = "اميديه",
                            StateId = 13
                        },
                        new
                        {
                            Id = 170,
                            Name = "گتوند",
                            StateId = 13
                        },
                        new
                        {
                            Id = 171,
                            Name = "شادگان",
                            StateId = 13
                        },
                        new
                        {
                            Id = 172,
                            Name = "دزفول",
                            StateId = 13
                        },
                        new
                        {
                            Id = 173,
                            Name = "رامشير",
                            StateId = 13
                        },
                        new
                        {
                            Id = 174,
                            Name = "بهبهان",
                            StateId = 13
                        },
                        new
                        {
                            Id = 175,
                            Name = "باوي",
                            StateId = 13
                        },
                        new
                        {
                            Id = 176,
                            Name = "انديمشك",
                            StateId = 13
                        },
                        new
                        {
                            Id = 177,
                            Name = "اهواز",
                            StateId = 13
                        },
                        new
                        {
                            Id = 178,
                            Name = "انديکا",
                            StateId = 13
                        },
                        new
                        {
                            Id = 179,
                            Name = "شوش",
                            StateId = 13
                        },
                        new
                        {
                            Id = 180,
                            Name = "آبادان",
                            StateId = 13
                        },
                        new
                        {
                            Id = 181,
                            Name = "هنديجان",
                            StateId = 13
                        },
                        new
                        {
                            Id = 182,
                            Name = "خرمشهر",
                            StateId = 13
                        },
                        new
                        {
                            Id = 183,
                            Name = "مسجد سليمان",
                            StateId = 13
                        },
                        new
                        {
                            Id = 184,
                            Name = "ايذه",
                            StateId = 13
                        },
                        new
                        {
                            Id = 185,
                            Name = "رامهرمز",
                            StateId = 13
                        },
                        new
                        {
                            Id = 186,
                            Name = "باغ ملك",
                            StateId = 13
                        },
                        new
                        {
                            Id = 187,
                            Name = "هفتکل",
                            StateId = 13
                        },
                        new
                        {
                            Id = 188,
                            Name = "هويزه",
                            StateId = 13
                        },
                        new
                        {
                            Id = 189,
                            Name = "ماهشهر",
                            StateId = 13
                        },
                        new
                        {
                            Id = 190,
                            Name = "لالي",
                            StateId = 13
                        },
                        new
                        {
                            Id = 191,
                            Name = "زنجان",
                            StateId = 14
                        },
                        new
                        {
                            Id = 192,
                            Name = "ابهر",
                            StateId = 14
                        },
                        new
                        {
                            Id = 193,
                            Name = "خدابنده",
                            StateId = 14
                        },
                        new
                        {
                            Id = 194,
                            Name = "ماهنشان",
                            StateId = 14
                        },
                        new
                        {
                            Id = 195,
                            Name = "خرمدره",
                            StateId = 14
                        },
                        new
                        {
                            Id = 196,
                            Name = "ايجرود",
                            StateId = 14
                        },
                        new
                        {
                            Id = 197,
                            Name = "طارم",
                            StateId = 14
                        },
                        new
                        {
                            Id = 198,
                            Name = "سلطانيه",
                            StateId = 14
                        },
                        new
                        {
                            Id = 199,
                            Name = "سمنان",
                            StateId = 15
                        },
                        new
                        {
                            Id = 200,
                            Name = "شاهرود",
                            StateId = 15
                        },
                        new
                        {
                            Id = 201,
                            Name = "گرمسار",
                            StateId = 15
                        },
                        new
                        {
                            Id = 202,
                            Name = "سرخه",
                            StateId = 15
                        },
                        new
                        {
                            Id = 203,
                            Name = "دامغان",
                            StateId = 15
                        },
                        new
                        {
                            Id = 204,
                            Name = "آرادان",
                            StateId = 15
                        },
                        new
                        {
                            Id = 205,
                            Name = "مهدي شهر",
                            StateId = 15
                        },
                        new
                        {
                            Id = 206,
                            Name = "ميامي",
                            StateId = 15
                        },
                        new
                        {
                            Id = 207,
                            Name = "زاهدان",
                            StateId = 16
                        },
                        new
                        {
                            Id = 208,
                            Name = "بمپور",
                            StateId = 16
                        },
                        new
                        {
                            Id = 209,
                            Name = "چابهار",
                            StateId = 16
                        },
                        new
                        {
                            Id = 210,
                            Name = "خاش",
                            StateId = 16
                        },
                        new
                        {
                            Id = 211,
                            Name = "سراوان",
                            StateId = 16
                        },
                        new
                        {
                            Id = 212,
                            Name = "زابل",
                            StateId = 16
                        },
                        new
                        {
                            Id = 213,
                            Name = "سرباز",
                            StateId = 16
                        },
                        new
                        {
                            Id = 214,
                            Name = "قصر قند",
                            StateId = 16
                        },
                        new
                        {
                            Id = 215,
                            Name = "نيكشهر",
                            StateId = 16
                        },
                        new
                        {
                            Id = 216,
                            Name = "کنارک",
                            StateId = 16
                        },
                        new
                        {
                            Id = 217,
                            Name = "ايرانشهر",
                            StateId = 16
                        },
                        new
                        {
                            Id = 218,
                            Name = "زهک",
                            StateId = 16
                        },
                        new
                        {
                            Id = 219,
                            Name = "سيب و سوران",
                            StateId = 16
                        },
                        new
                        {
                            Id = 220,
                            Name = "ميرجاوه",
                            StateId = 16
                        },
                        new
                        {
                            Id = 221,
                            Name = "دلگان",
                            StateId = 16
                        },
                        new
                        {
                            Id = 222,
                            Name = "هيرمند",
                            StateId = 16
                        },
                        new
                        {
                            Id = 223,
                            Name = "مهرستان",
                            StateId = 16
                        },
                        new
                        {
                            Id = 224,
                            Name = "فنوج",
                            StateId = 16
                        },
                        new
                        {
                            Id = 225,
                            Name = "هامون",
                            StateId = 16
                        },
                        new
                        {
                            Id = 226,
                            Name = "نيمروز",
                            StateId = 16
                        },
                        new
                        {
                            Id = 227,
                            Name = "شيراز",
                            StateId = 17
                        },
                        new
                        {
                            Id = 228,
                            Name = "اقليد",
                            StateId = 17
                        },
                        new
                        {
                            Id = 229,
                            Name = "داراب",
                            StateId = 17
                        },
                        new
                        {
                            Id = 230,
                            Name = "فسا	",
                            StateId = 17
                        },
                        new
                        {
                            Id = 231,
                            Name = "مرودشت",
                            StateId = 17
                        },
                        new
                        {
                            Id = 232,
                            Name = "خرم بيد",
                            StateId = 17
                        },
                        new
                        {
                            Id = 233,
                            Name = "آباده",
                            StateId = 17
                        },
                        new
                        {
                            Id = 234,
                            Name = "كازرون",
                            StateId = 17
                        },
                        new
                        {
                            Id = 235,
                            Name = "گراش",
                            StateId = 17
                        },
                        new
                        {
                            Id = 236,
                            Name = "ممسني",
                            StateId = 17
                        },
                        new
                        {
                            Id = 237,
                            Name = "سپيدان",
                            StateId = 17
                        },
                        new
                        {
                            Id = 238,
                            Name = "لارستان",
                            StateId = 17
                        },
                        new
                        {
                            Id = 239,
                            Name = "فيروز آباد",
                            StateId = 17
                        },
                        new
                        {
                            Id = 240,
                            Name = "جهرم",
                            StateId = 17
                        },
                        new
                        {
                            Id = 241,
                            Name = "ني ريز",
                            StateId = 17
                        },
                        new
                        {
                            Id = 242,
                            Name = "استهبان",
                            StateId = 17
                        },
                        new
                        {
                            Id = 243,
                            Name = "لامرد",
                            StateId = 17
                        },
                        new
                        {
                            Id = 244,
                            Name = "مهر",
                            StateId = 17
                        },
                        new
                        {
                            Id = 245,
                            Name = "پاسارگاد",
                            StateId = 17
                        },
                        new
                        {
                            Id = 246,
                            Name = "ارسنجان",
                            StateId = 17
                        },
                        new
                        {
                            Id = 247,
                            Name = "قيروكارزين",
                            StateId = 17
                        },
                        new
                        {
                            Id = 248,
                            Name = "رستم",
                            StateId = 17
                        },
                        new
                        {
                            Id = 249,
                            Name = "فراشبند",
                            StateId = 17
                        },
                        new
                        {
                            Id = 250,
                            Name = "سروستان",
                            StateId = 17
                        },
                        new
                        {
                            Id = 251,
                            Name = "زرين دشت",
                            StateId = 17
                        },
                        new
                        {
                            Id = 252,
                            Name = "کوار",
                            StateId = 17
                        },
                        new
                        {
                            Id = 253,
                            Name = "بوانات",
                            StateId = 17
                        },
                        new
                        {
                            Id = 254,
                            Name = "خرامه",
                            StateId = 17
                        },
                        new
                        {
                            Id = 255,
                            Name = "خنج",
                            StateId = 17
                        },
                        new
                        {
                            Id = 256,
                            Name = "قزوين",
                            StateId = 18
                        },
                        new
                        {
                            Id = 257,
                            Name = "تاكستان",
                            StateId = 18
                        },
                        new
                        {
                            Id = 258,
                            Name = "آبيك",
                            StateId = 18
                        },
                        new
                        {
                            Id = 259,
                            Name = "بوئين زهرا",
                            StateId = 18
                        },
                        new
                        {
                            Id = 260,
                            Name = "البرز",
                            StateId = 18
                        },
                        new
                        {
                            Id = 261,
                            Name = "آوج",
                            StateId = 18
                        },
                        new
                        {
                            Id = 262,
                            Name = "قم",
                            StateId = 19
                        },
                        new
                        {
                            Id = 263,
                            Name = "طالقان",
                            StateId = 5
                        },
                        new
                        {
                            Id = 264,
                            Name = "اشتهارد",
                            StateId = 5
                        },
                        new
                        {
                            Id = 265,
                            Name = "كرج",
                            StateId = 5
                        },
                        new
                        {
                            Id = 266,
                            Name = "نظر آباد",
                            StateId = 5
                        },
                        new
                        {
                            Id = 267,
                            Name = "ساوجبلاغ‎",
                            StateId = 5
                        },
                        new
                        {
                            Id = 268,
                            Name = "فرديس",
                            StateId = 5
                        },
                        new
                        {
                            Id = 269,
                            Name = "سنندج",
                            StateId = 20
                        },
                        new
                        {
                            Id = 270,
                            Name = "ديواندره",
                            StateId = 20
                        },
                        new
                        {
                            Id = 271,
                            Name = "بانه",
                            StateId = 20
                        },
                        new
                        {
                            Id = 272,
                            Name = "بيجار",
                            StateId = 20
                        },
                        new
                        {
                            Id = 273,
                            Name = "سقز",
                            StateId = 20
                        },
                        new
                        {
                            Id = 274,
                            Name = "كامياران",
                            StateId = 20
                        },
                        new
                        {
                            Id = 275,
                            Name = "قروه",
                            StateId = 20
                        },
                        new
                        {
                            Id = 276,
                            Name = "مريوان",
                            StateId = 20
                        },
                        new
                        {
                            Id = 277,
                            Name = "سروآباد",
                            StateId = 20
                        },
                        new
                        {
                            Id = 278,
                            Name = "دهگلان‎",
                            StateId = 20
                        },
                        new
                        {
                            Id = 279,
                            Name = "كرمان",
                            StateId = 21
                        },
                        new
                        {
                            Id = 280,
                            Name = "راور",
                            StateId = 21
                        },
                        new
                        {
                            Id = 281,
                            Name = "شهر بابک",
                            StateId = 21
                        },
                        new
                        {
                            Id = 282,
                            Name = "انار",
                            StateId = 21
                        },
                        new
                        {
                            Id = 283,
                            Name = "کوهبنان",
                            StateId = 21
                        },
                        new
                        {
                            Id = 284,
                            Name = "رفسنجان",
                            StateId = 21
                        },
                        new
                        {
                            Id = 285,
                            Name = "سيرجان",
                            StateId = 21
                        },
                        new
                        {
                            Id = 286,
                            Name = "كهنوج",
                            StateId = 21
                        },
                        new
                        {
                            Id = 287,
                            Name = "زرند",
                            StateId = 21
                        },
                        new
                        {
                            Id = 288,
                            Name = "ريگان",
                            StateId = 21
                        },
                        new
                        {
                            Id = 289,
                            Name = "بم",
                            StateId = 21
                        },
                        new
                        {
                            Id = 290,
                            Name = "جيرفت",
                            StateId = 21
                        },
                        new
                        {
                            Id = 291,
                            Name = "عنبرآباد",
                            StateId = 21
                        },
                        new
                        {
                            Id = 292,
                            Name = "بافت",
                            StateId = 21
                        },
                        new
                        {
                            Id = 293,
                            Name = "ارزوئيه",
                            StateId = 21
                        },
                        new
                        {
                            Id = 294,
                            Name = "بردسير",
                            StateId = 21
                        },
                        new
                        {
                            Id = 295,
                            Name = "فهرج",
                            StateId = 21
                        },
                        new
                        {
                            Id = 296,
                            Name = "فارياب",
                            StateId = 21
                        },
                        new
                        {
                            Id = 297,
                            Name = "منوجان",
                            StateId = 21
                        },
                        new
                        {
                            Id = 298,
                            Name = "نرماشير",
                            StateId = 21
                        },
                        new
                        {
                            Id = 299,
                            Name = "قلعه گنج",
                            StateId = 21
                        },
                        new
                        {
                            Id = 300,
                            Name = "رابر",
                            StateId = 21
                        },
                        new
                        {
                            Id = 301,
                            Name = "رودبار جنوب",
                            StateId = 21
                        },
                        new
                        {
                            Id = 302,
                            Name = "كرمانشاه",
                            StateId = 22
                        },
                        new
                        {
                            Id = 303,
                            Name = "اسلام آباد غرب",
                            StateId = 22
                        },
                        new
                        {
                            Id = 304,
                            Name = "سر پل ذهاب",
                            StateId = 22
                        },
                        new
                        {
                            Id = 305,
                            Name = "كنگاور",
                            StateId = 22
                        },
                        new
                        {
                            Id = 306,
                            Name = "سنقر",
                            StateId = 22
                        },
                        new
                        {
                            Id = 307,
                            Name = "قصر شيرين",
                            StateId = 22
                        },
                        new
                        {
                            Id = 308,
                            Name = "گيلان غرب",
                            StateId = 22
                        },
                        new
                        {
                            Id = 309,
                            Name = "هرسين",
                            StateId = 22
                        },
                        new
                        {
                            Id = 310,
                            Name = "صحنه",
                            StateId = 22
                        },
                        new
                        {
                            Id = 311,
                            Name = "پاوه",
                            StateId = 22
                        },
                        new
                        {
                            Id = 312,
                            Name = "جوانرود",
                            StateId = 22
                        },
                        new
                        {
                            Id = 313,
                            Name = "دالاهو",
                            StateId = 22
                        },
                        new
                        {
                            Id = 314,
                            Name = "روانسر",
                            StateId = 22
                        },
                        new
                        {
                            Id = 315,
                            Name = "ثلاث باباجاني",
                            StateId = 22
                        },
                        new
                        {
                            Id = 316,
                            Name = "ياسوج",
                            StateId = 23
                        },
                        new
                        {
                            Id = 317,
                            Name = "گچساران",
                            StateId = 23
                        },
                        new
                        {
                            Id = 318,
                            Name = "دنا",
                            StateId = 23
                        },
                        new
                        {
                            Id = 319,
                            Name = "کهگيلويه‎",
                            StateId = 23
                        },
                        new
                        {
                            Id = 320,
                            Name = "لنده",
                            StateId = 23
                        },
                        new
                        {
                            Id = 321,
                            Name = "بهمئي",
                            StateId = 23
                        },
                        new
                        {
                            Id = 322,
                            Name = "باشت",
                            StateId = 23
                        },
                        new
                        {
                            Id = 323,
                            Name = "بويراحمد",
                            StateId = 23
                        },
                        new
                        {
                            Id = 324,
                            Name = "چرام",
                            StateId = 23
                        },
                        new
                        {
                            Id = 325,
                            Name = "گرگان",
                            StateId = 24
                        },
                        new
                        {
                            Id = 326,
                            Name = "آق قلا",
                            StateId = 24
                        },
                        new
                        {
                            Id = 327,
                            Name = "گنبد كاووس",
                            StateId = 24
                        },
                        new
                        {
                            Id = 328,
                            Name = "علي آباد",
                            StateId = 24
                        },
                        new
                        {
                            Id = 329,
                            Name = "مينو دشت",
                            StateId = 24
                        },
                        new
                        {
                            Id = 330,
                            Name = "تركمن",
                            StateId = 24
                        },
                        new
                        {
                            Id = 331,
                            Name = "كردكوي",
                            StateId = 24
                        },
                        new
                        {
                            Id = 332,
                            Name = "بندر گز",
                            StateId = 24
                        },
                        new
                        {
                            Id = 333,
                            Name = "كلاله",
                            StateId = 24
                        },
                        new
                        {
                            Id = 334,
                            Name = "آزاد شهر",
                            StateId = 24
                        },
                        new
                        {
                            Id = 335,
                            Name = "راميان",
                            StateId = 24
                        },
                        new
                        {
                            Id = 336,
                            Name = "گاليکش‎",
                            StateId = 24
                        },
                        new
                        {
                            Id = 337,
                            Name = "مراوه تپه",
                            StateId = 24
                        },
                        new
                        {
                            Id = 338,
                            Name = "گميشان",
                            StateId = 24
                        },
                        new
                        {
                            Id = 339,
                            Name = "رشت",
                            StateId = 25
                        },
                        new
                        {
                            Id = 340,
                            Name = "لنگرود",
                            StateId = 25
                        },
                        new
                        {
                            Id = 341,
                            Name = "رودسر",
                            StateId = 25
                        },
                        new
                        {
                            Id = 342,
                            Name = "طوالش",
                            StateId = 25
                        },
                        new
                        {
                            Id = 343,
                            Name = "آستارا",
                            StateId = 25
                        },
                        new
                        {
                            Id = 344,
                            Name = "آستانه اشرفيه",
                            StateId = 25
                        },
                        new
                        {
                            Id = 345,
                            Name = "رودبار",
                            StateId = 25
                        },
                        new
                        {
                            Id = 346,
                            Name = "فومن",
                            StateId = 25
                        },
                        new
                        {
                            Id = 347,
                            Name = "صومعه سرا",
                            StateId = 25
                        },
                        new
                        {
                            Id = 348,
                            Name = "بندرانزلي",
                            StateId = 25
                        },
                        new
                        {
                            Id = 349,
                            Name = "رضوانشهر",
                            StateId = 25
                        },
                        new
                        {
                            Id = 350,
                            Name = "ماسال",
                            StateId = 25
                        },
                        new
                        {
                            Id = 351,
                            Name = "شفت",
                            StateId = 25
                        },
                        new
                        {
                            Id = 352,
                            Name = "سياهكل",
                            StateId = 25
                        },
                        new
                        {
                            Id = 353,
                            Name = "املش",
                            StateId = 25
                        },
                        new
                        {
                            Id = 354,
                            Name = "لاهيجان",
                            StateId = 25
                        },
                        new
                        {
                            Id = 355,
                            Name = "خرم آباد",
                            StateId = 26
                        },
                        new
                        {
                            Id = 356,
                            Name = "دلفان",
                            StateId = 26
                        },
                        new
                        {
                            Id = 357,
                            Name = "بروجرد",
                            StateId = 26
                        },
                        new
                        {
                            Id = 358,
                            Name = "دورود",
                            StateId = 26
                        },
                        new
                        {
                            Id = 359,
                            Name = "اليگودرز",
                            StateId = 26
                        },
                        new
                        {
                            Id = 360,
                            Name = "ازنا",
                            StateId = 26
                        },
                        new
                        {
                            Id = 361,
                            Name = "كوهدشت",
                            StateId = 26
                        },
                        new
                        {
                            Id = 362,
                            Name = "سلسله",
                            StateId = 26
                        },
                        new
                        {
                            Id = 363,
                            Name = "پلدختر",
                            StateId = 26
                        },
                        new
                        {
                            Id = 364,
                            Name = "دوره",
                            StateId = 26
                        },
                        new
                        {
                            Id = 365,
                            Name = "رومشکان",
                            StateId = 26
                        },
                        new
                        {
                            Id = 366,
                            Name = "ساري",
                            StateId = 27
                        },
                        new
                        {
                            Id = 367,
                            Name = "آمل",
                            StateId = 27
                        },
                        new
                        {
                            Id = 368,
                            Name = "بابل",
                            StateId = 27
                        },
                        new
                        {
                            Id = 369,
                            Name = "بابلسر",
                            StateId = 27
                        },
                        new
                        {
                            Id = 370,
                            Name = "بهشهر",
                            StateId = 27
                        },
                        new
                        {
                            Id = 371,
                            Name = "تنكابن",
                            StateId = 27
                        },
                        new
                        {
                            Id = 372,
                            Name = "جويبار",
                            StateId = 27
                        },
                        new
                        {
                            Id = 373,
                            Name = "چالوس",
                            StateId = 27
                        },
                        new
                        {
                            Id = 374,
                            Name = "رامسر",
                            StateId = 27
                        },
                        new
                        {
                            Id = 375,
                            Name = "سواد كوه",
                            StateId = 27
                        },
                        new
                        {
                            Id = 376,
                            Name = "قائم شهر",
                            StateId = 27
                        },
                        new
                        {
                            Id = 377,
                            Name = "نكا",
                            StateId = 27
                        },
                        new
                        {
                            Id = 378,
                            Name = "نور",
                            StateId = 27
                        },
                        new
                        {
                            Id = 379,
                            Name = "نوشهر",
                            StateId = 27
                        },
                        new
                        {
                            Id = 380,
                            Name = "محمودآباد",
                            StateId = 27
                        },
                        new
                        {
                            Id = 381,
                            Name = "فريدونکنار",
                            StateId = 27
                        },
                        new
                        {
                            Id = 382,
                            Name = "عباس آباد",
                            StateId = 27
                        },
                        new
                        {
                            Id = 383,
                            Name = "گلوگاه",
                            StateId = 27
                        },
                        new
                        {
                            Id = 384,
                            Name = "مياندورود",
                            StateId = 27
                        },
                        new
                        {
                            Id = 385,
                            Name = "سيمرغ",
                            StateId = 27
                        },
                        new
                        {
                            Id = 386,
                            Name = "کلاردشت",
                            StateId = 27
                        },
                        new
                        {
                            Id = 387,
                            Name = "سوادکوه شمالي",
                            StateId = 27
                        },
                        new
                        {
                            Id = 388,
                            Name = "اراك",
                            StateId = 28
                        },
                        new
                        {
                            Id = 389,
                            Name = "آشتيان",
                            StateId = 28
                        },
                        new
                        {
                            Id = 390,
                            Name = "تفرش",
                            StateId = 28
                        },
                        new
                        {
                            Id = 391,
                            Name = "خمين",
                            StateId = 28
                        },
                        new
                        {
                            Id = 392,
                            Name = "دليجان",
                            StateId = 28
                        },
                        new
                        {
                            Id = 393,
                            Name = "ساوه",
                            StateId = 28
                        },
                        new
                        {
                            Id = 394,
                            Name = "زرنديه",
                            StateId = 28
                        },
                        new
                        {
                            Id = 395,
                            Name = "محلات",
                            StateId = 28
                        },
                        new
                        {
                            Id = 396,
                            Name = "شازند",
                            StateId = 28
                        },
                        new
                        {
                            Id = 397,
                            Name = "فراهان",
                            StateId = 28
                        },
                        new
                        {
                            Id = 398,
                            Name = "خنداب",
                            StateId = 28
                        },
                        new
                        {
                            Id = 399,
                            Name = "کميجان",
                            StateId = 28
                        },
                        new
                        {
                            Id = 400,
                            Name = "بندرعباس",
                            StateId = 29
                        },
                        new
                        {
                            Id = 401,
                            Name = "قشم",
                            StateId = 29
                        },
                        new
                        {
                            Id = 402,
                            Name = "بندر لنگه",
                            StateId = 29
                        },
                        new
                        {
                            Id = 403,
                            Name = "بستك",
                            StateId = 29
                        },
                        new
                        {
                            Id = 404,
                            Name = "حاجي آباد هرمزگان",
                            StateId = 29
                        },
                        new
                        {
                            Id = 405,
                            Name = "رودان",
                            StateId = 29
                        },
                        new
                        {
                            Id = 406,
                            Name = "ميناب",
                            StateId = 29
                        },
                        new
                        {
                            Id = 407,
                            Name = "ابوموسي",
                            StateId = 29
                        },
                        new
                        {
                            Id = 408,
                            Name = "جاسک",
                            StateId = 29
                        },
                        new
                        {
                            Id = 409,
                            Name = "خمير",
                            StateId = 29
                        },
                        new
                        {
                            Id = 410,
                            Name = "پارسيان",
                            StateId = 29
                        },
                        new
                        {
                            Id = 411,
                            Name = "بشاگرد",
                            StateId = 29
                        },
                        new
                        {
                            Id = 412,
                            Name = "سيريک",
                            StateId = 29
                        },
                        new
                        {
                            Id = 413,
                            Name = "حاجي آباد",
                            StateId = 29
                        },
                        new
                        {
                            Id = 414,
                            Name = "همدان",
                            StateId = 30
                        },
                        new
                        {
                            Id = 415,
                            Name = "ملاير",
                            StateId = 30
                        },
                        new
                        {
                            Id = 416,
                            Name = "تويسركان",
                            StateId = 30
                        },
                        new
                        {
                            Id = 417,
                            Name = "نهاوند",
                            StateId = 30
                        },
                        new
                        {
                            Id = 418,
                            Name = "كبودر اهنگ",
                            StateId = 30
                        },
                        new
                        {
                            Id = 419,
                            Name = "رزن",
                            StateId = 30
                        },
                        new
                        {
                            Id = 420,
                            Name = "اسدآباد",
                            StateId = 30
                        },
                        new
                        {
                            Id = 421,
                            Name = "بهار",
                            StateId = 30
                        },
                        new
                        {
                            Id = 422,
                            Name = "فامنين",
                            StateId = 30
                        },
                        new
                        {
                            Id = 423,
                            Name = "يزد",
                            StateId = 31
                        },
                        new
                        {
                            Id = 424,
                            Name = "تفت",
                            StateId = 31
                        },
                        new
                        {
                            Id = 425,
                            Name = "اردكان",
                            StateId = 31
                        },
                        new
                        {
                            Id = 426,
                            Name = "ابركوه",
                            StateId = 31
                        },
                        new
                        {
                            Id = 427,
                            Name = "ميبد",
                            StateId = 31
                        },
                        new
                        {
                            Id = 428,
                            Name = "بافق",
                            StateId = 31
                        },
                        new
                        {
                            Id = 429,
                            Name = "صدوق",
                            StateId = 31
                        },
                        new
                        {
                            Id = 430,
                            Name = "مهريز",
                            StateId = 31
                        },
                        new
                        {
                            Id = 431,
                            Name = "خاتم",
                            StateId = 31
                        });
                });

            modelBuilder.Entity("Entities.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ColorCode")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Colors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ColorCode = "#FFFFFF",
                            Name = "بدون رنگ بندی"
                        });
                });

            modelBuilder.Entity("Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 1,
                            Name = "تومان"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 24000,
                            Name = "دلار"
                        });
                });

            modelBuilder.Entity("Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Location")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Entities.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MaxOrder")
                        .HasColumnType("int");

                    b.Property<int?>("MinOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Percent")
                        .HasColumnType("float");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Discounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "NoDiscount",
                            IsActive = false,
                            Name = "بدون تخفیف"
                        });
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Commission")
                        .HasColumnType("int");

                    b.Property<string>("CustomerCode")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CustomerCodeCustomer")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("HireDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("HolooCompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("HolooCompanyId");

                    b.HasIndex("UserRoleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Entities.HolooCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ConnectionString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("HolooCompanies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConnectionString = "Server=.\\mssql2008r2;Database=Holoo1;Trusted_Connection=True;MultipleActiveResultSets=true;",
                            Name = "Holoo1"
                        });
                });

            modelBuilder.Entity("Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Alt")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("BlogId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlogId")
                        .IsUnique()
                        .HasFilter("[BlogId] IS NOT NULL");

                    b.HasIndex("ProductId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Entities.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("KeywordText")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("Entities.LoginHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LoginHistories");
                });

            modelBuilder.Entity("Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Entities.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BankCode")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BrunchName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PaymentMethodStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("Entities.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ArticleCode")
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("ArticleCodeCustomer")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ColorId")
                        .HasColumnType("int");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<double>("Exist")
                        .HasColumnType("float");

                    b.Property<byte>("Grade")
                        .HasColumnType("tinyint");

                    b.Property<bool>("IsColleague")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<int>("MaxQuantity")
                        .HasColumnType("int");

                    b.Property<int>("MinQuantity")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("SellNumber")
                        .HasColumnType("int");

                    b.Property<int?>("SizeId")
                        .HasColumnType("int");

                    b.Property<int?>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.HasIndex("UnitId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HolooCompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDiscontinued")
                        .HasColumnType("bit");

                    b.Property<bool>("IsShowInIndexPage")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxOrder")
                        .HasColumnType("int");

                    b.Property<double?>("MinInStore")
                        .HasColumnType("float");

                    b.Property<int>("MinOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("ReorderingLevel")
                        .HasColumnType("int");

                    b.Property<string>("Review")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Star")
                        .HasColumnType("float");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("HolooCompanyId");

                    b.HasIndex("StoreId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Entities.ProductAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AttributeGroupId")
                        .HasColumnType("int");

                    b.Property<int>("AttributeType")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("AttributeGroupId");

                    b.ToTable("ProductAttributes");
                });

            modelBuilder.Entity("Entities.ProductAttributeGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ProductAttributeGroups");
                });

            modelBuilder.Entity("Entities.ProductAttributeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ProductAttributeId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductAttributeId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAttributeValues");
                });

            modelBuilder.Entity("Entities.ProductComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAnswered")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductComments");
                });

            modelBuilder.Entity("Entities.ProductSellCount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSellCounts");
                });

            modelBuilder.Entity("Entities.ProductUserRank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductUserRanks");
                });

            modelBuilder.Entity("Entities.PurchaseOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("AccountantDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AccountantId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ApprovedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DiscountAmount")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpectedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FBailCode")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<Guid>("OrderGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int?>("SendInformationId")
                        .HasColumnType("int");

                    b.Property<int?>("ShippingFee")
                        .HasColumnType("int");

                    b.Property<int?>("ShippingId")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("SubmittedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SubmittedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Taxes")
                        .HasColumnType("int");

                    b.Property<int?>("TransactionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountantId");

                    b.HasIndex("ApprovedById");

                    b.HasIndex("DiscountId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("SendInformationId");

                    b.HasIndex("ShippingId");

                    b.HasIndex("SubmittedById");

                    b.HasIndex("TransactionId");

                    b.HasIndex("UserId");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("Entities.PurchaseOrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("DiscountAmount")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PriceId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("PurchaseOrderId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("SumPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("PriceId");

                    b.HasIndex("ProductId");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("PurchaseOrderDetails");
                });

            modelBuilder.Entity("Entities.SendInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("RecipientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("StateId");

                    b.HasIndex("UserId");

                    b.ToTable("SendInformation");
                });

            modelBuilder.Entity("Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Currency",
                            Value = "تومان"
                        });
                });

            modelBuilder.Entity("Entities.Shipping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Shipping");
                });

            modelBuilder.Entity("Entities.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Sizes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "بدون سایز بندی"
                        });
                });

            modelBuilder.Entity("Entities.SlideShow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("SlideShows");
                });

            modelBuilder.Entity("Entities.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("States");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "آذربايجان شرقي"
                        },
                        new
                        {
                            Id = 2,
                            Name = "آذربايجان غربي"
                        },
                        new
                        {
                            Id = 3,
                            Name = "اردبيل"
                        },
                        new
                        {
                            Id = 4,
                            Name = "اصفهان"
                        },
                        new
                        {
                            Id = 5,
                            Name = "البرز"
                        },
                        new
                        {
                            Id = 6,
                            Name = "ايلام"
                        },
                        new
                        {
                            Id = 7,
                            Name = "بوشهر"
                        },
                        new
                        {
                            Id = 8,
                            Name = "تهران"
                        },
                        new
                        {
                            Id = 9,
                            Name = "چهارمحال و بختياري"
                        },
                        new
                        {
                            Id = 10,
                            Name = "خراسان جنوبي"
                        },
                        new
                        {
                            Id = 11,
                            Name = "خراسان رضوي"
                        },
                        new
                        {
                            Id = 12,
                            Name = "خراسان شمالي"
                        },
                        new
                        {
                            Id = 13,
                            Name = "خوزستان"
                        },
                        new
                        {
                            Id = 14,
                            Name = "زنجان"
                        },
                        new
                        {
                            Id = 15,
                            Name = "سمنان"
                        },
                        new
                        {
                            Id = 16,
                            Name = "سيستان و بلوچستان"
                        },
                        new
                        {
                            Id = 17,
                            Name = "فارس"
                        },
                        new
                        {
                            Id = 18,
                            Name = "قزوين"
                        },
                        new
                        {
                            Id = 19,
                            Name = "قم"
                        },
                        new
                        {
                            Id = 20,
                            Name = "کردستان"
                        },
                        new
                        {
                            Id = 21,
                            Name = "کرمان"
                        },
                        new
                        {
                            Id = 22,
                            Name = "کرمانشاه"
                        },
                        new
                        {
                            Id = 23,
                            Name = "کهکيلويه و بويراحمد"
                        },
                        new
                        {
                            Id = 24,
                            Name = "گلستان"
                        },
                        new
                        {
                            Id = 25,
                            Name = "گيلان"
                        },
                        new
                        {
                            Id = 26,
                            Name = "لرستان"
                        },
                        new
                        {
                            Id = 27,
                            Name = "مازندران"
                        },
                        new
                        {
                            Id = 28,
                            Name = "مرکزي"
                        },
                        new
                        {
                            Id = 29,
                            Name = "هرمزگان"
                        },
                        new
                        {
                            Id = 30,
                            Name = "همدان"
                        },
                        new
                        {
                            Id = 31,
                            Name = "يزد"
                        });
                });

            modelBuilder.Entity("Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Stores");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "انبار پیش فرض"
                        });
                });

            modelBuilder.Entity("Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectionName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Mobile")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tell")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Website")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "تامین کننده پیش فرض"
                        });
                });

            modelBuilder.Entity("Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("TagText")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("HolooCompanyId")
                        .HasColumnType("int");

                    b.Property<double?>("PaymentId")
                        .HasColumnType("float");

                    b.Property<int?>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<string>("RefId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("SanadCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HolooCompanyId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Entities.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Few")
                        .HasColumnType("float");

                    b.Property<int?>("HolooCompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("UnitCode")
                        .HasColumnType("int");

                    b.Property<short?>("UnitWeight")
                        .HasColumnType("smallint");

                    b.Property<double?>("assay")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("HolooCompanyId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerCode")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CustomerCodeCustomer")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FatherName")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("HolooCompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsColleague")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConfirmedColleague")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFeeder")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHaveCustomerCode")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LicensePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("NationalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StateId")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("HolooCompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("StateId");

                    b.HasIndex("UserRoleId");

                    b.ToTable("Users", "Security");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            Birthday = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ConcurrencyStamp = "4cedc9d2-f7e6-4bcd-850a-d18ee7e03128",
                            Email = "sayyah.alireza@gmail.com",
                            EmailConfirmed = true,
                            FirstName = "Alireza",
                            HolooCompanyId = 1,
                            IsActive = true,
                            IsColleague = false,
                            IsConfirmedColleague = false,
                            IsFeeder = false,
                            IsHaveCustomerCode = false,
                            LastName = "Sayyah",
                            LockoutEnabled = false,
                            Mobile = "No Mobile",
                            NormalizedEmail = "SAYYAH.ALIREZA@GMAIL.COM",
                            NormalizedUserName = "SUPERADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAEHVjuDHUMflY1WgCdRDCGsVfRowfJKuCvQwpp40++Voy48WjWdtVG5OR1+2a+CNchA==",
                            PhoneNumber = "0911307006",
                            PhoneNumberConfirmed = true,
                            RegisterDate = new DateTime(2022, 11, 15, 8, 16, 37, 92, DateTimeKind.Local).AddTicks(5619),
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "superadmin",
                            UserRoleId = 1
                        });
                });

            modelBuilder.Entity("Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("UserRoles", "Security");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "c54b6260-a1c3-49dd-9009-52e6c0c72acc",
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        },
                        new
                        {
                            Id = 2,
                            ConcurrencyStamp = "2f48e490-b9bd-415f-8138-e9243b561cdd",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 4,
                            ConcurrencyStamp = "103dfbae-ff42-41ef-86da-e428b871a64f",
                            Name = "Client",
                            NormalizedName = "Client"
                        });
                });

            modelBuilder.Entity("Entities.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("WishLists");
                });

            modelBuilder.Entity("KeywordProduct", b =>
                {
                    b.Property<int>("KeywordsId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("KeywordsId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("KeywordProduct");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim", "Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim", "Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin", "Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", "Security");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken", "Security");
                });

            modelBuilder.Entity("ProductProductAttributeGroup", b =>
                {
                    b.Property<int>("AttributeGroupProductsId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("AttributeGroupProductsId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("ProductProductAttributeGroup");
                });

            modelBuilder.Entity("ProductTag", b =>
                {
                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.HasKey("ProductsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("ProductTag");
                });

            modelBuilder.Entity("BlogKeyword", b =>
                {
                    b.HasOne("Entities.Blog", null)
                        .WithMany()
                        .HasForeignKey("BlogsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Keyword", null)
                        .WithMany()
                        .HasForeignKey("KeywordsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogTag", b =>
                {
                    b.HasOne("Entities.Blog", null)
                        .WithMany()
                        .HasForeignKey("BlogsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("ProductCategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Blog", b =>
                {
                    b.HasOne("Entities.BlogAuthor", "BlogAuthor")
                        .WithMany("Blogs")
                        .HasForeignKey("BlogAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.BlogCategory", "BlogCategory")
                        .WithMany("Blogs")
                        .HasForeignKey("BlogCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlogAuthor");

                    b.Navigation("BlogCategory");
                });

            modelBuilder.Entity("Entities.BlogCategory", b =>
                {
                    b.HasOne("Entities.BlogCategory", "Parent")
                        .WithMany("BlogCategories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Entities.BlogComment", b =>
                {
                    b.HasOne("Entities.BlogComment", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId");

                    b.HasOne("Entities.Blog", "Blog")
                        .WithMany("BlogComments")
                        .HasForeignKey("BlogId");

                    b.HasOne("Entities.Employee", "Employee")
                        .WithMany("BlogComments")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("Entities.User", "User")
                        .WithMany("BlogComments")
                        .HasForeignKey("UserId");

                    b.Navigation("Answer");

                    b.Navigation("Blog");

                    b.Navigation("Employee");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.Category", b =>
                {
                    b.HasOne("Entities.Discount", "Discount")
                        .WithMany("Categories")
                        .HasForeignKey("DiscountId");

                    b.HasOne("Entities.Category", "Parent")
                        .WithMany("Categories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Discount");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Entities.City", b =>
                {
                    b.HasOne("Entities.State", "State")
                        .WithMany("Cities")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.HasOne("Entities.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("Entities.HolooCompany", "HolooCompany")
                        .WithMany()
                        .HasForeignKey("HolooCompanyId");

                    b.HasOne("Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("UserRoleId");

                    b.Navigation("Department");

                    b.Navigation("HolooCompany");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("Entities.Image", b =>
                {
                    b.HasOne("Entities.Blog", "Blog")
                        .WithOne("Image")
                        .HasForeignKey("Entities.Image", "BlogId");

                    b.HasOne("Entities.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId");

                    b.Navigation("Blog");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Entities.Message", b =>
                {
                    b.HasOne("Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.Price", b =>
                {
                    b.HasOne("Entities.Color", "Color")
                        .WithMany("Prices")
                        .HasForeignKey("ColorId");

                    b.HasOne("Entities.Currency", "Currency")
                        .WithMany("Prices")
                        .HasForeignKey("CurrencyId");

                    b.HasOne("Entities.Discount", "Discount")
                        .WithMany("Prices")
                        .HasForeignKey("DiscountId");

                    b.HasOne("Entities.Product", "Product")
                        .WithMany("Prices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Size", "Size")
                        .WithMany("Prices")
                        .HasForeignKey("SizeId");

                    b.HasOne("Entities.Unit", "Unit")
                        .WithMany("Prices")
                        .HasForeignKey("UnitId");

                    b.Navigation("Color");

                    b.Navigation("Currency");

                    b.Navigation("Discount");

                    b.Navigation("Product");

                    b.Navigation("Size");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Entities.Product", b =>
                {
                    b.HasOne("Entities.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId");

                    b.HasOne("Entities.HolooCompany", "HolooCompany")
                        .WithMany()
                        .HasForeignKey("HolooCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("HolooCompany");

                    b.Navigation("Store");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Entities.ProductAttribute", b =>
                {
                    b.HasOne("Entities.ProductAttributeGroup", "AttributeGroup")
                        .WithMany("Attribute")
                        .HasForeignKey("AttributeGroupId");

                    b.Navigation("AttributeGroup");
                });

            modelBuilder.Entity("Entities.ProductAttributeValue", b =>
                {
                    b.HasOne("Entities.ProductAttribute", "ProductAttribute")
                        .WithMany("AttributeValue")
                        .HasForeignKey("ProductAttributeId");

                    b.HasOne("Entities.Product", "Product")
                        .WithMany("AttributeValues")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");

                    b.Navigation("ProductAttribute");
                });

            modelBuilder.Entity("Entities.ProductComment", b =>
                {
                    b.HasOne("Entities.ProductComment", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId");

                    b.HasOne("Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("Entities.Product", "Product")
                        .WithMany("ProductComments")
                        .HasForeignKey("ProductId");

                    b.HasOne("Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Answer");

                    b.Navigation("Employee");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.ProductSellCount", b =>
                {
                    b.HasOne("Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Entities.ProductUserRank", b =>
                {
                    b.HasOne("Entities.Product", "Product")
                        .WithMany("ProductUserRanks")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.PurchaseOrder", b =>
                {
                    b.HasOne("Entities.Employee", "Accountant")
                        .WithMany("PurchaseOrderAccountant")
                        .HasForeignKey("AccountantId");

                    b.HasOne("Entities.Employee", "ApprovedBy")
                        .WithMany("PurchaseOrderApprovedBy")
                        .HasForeignKey("ApprovedById");

                    b.HasOne("Entities.Discount", "Discount")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("DiscountId");

                    b.HasOne("Entities.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("Entities.SendInformation", "SendInformation")
                        .WithMany()
                        .HasForeignKey("SendInformationId");

                    b.HasOne("Entities.Shipping", "Shipping")
                        .WithMany("PurchaseOrder")
                        .HasForeignKey("ShippingId");

                    b.HasOne("Entities.Employee", "SubmittedBy")
                        .WithMany("PurchaseOrderSubmittedBy")
                        .HasForeignKey("SubmittedById");

                    b.HasOne("Entities.Transaction", "Transaction")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("TransactionId");

                    b.HasOne("Entities.User", "User")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accountant");

                    b.Navigation("ApprovedBy");

                    b.Navigation("Discount");

                    b.Navigation("PaymentMethod");

                    b.Navigation("SendInformation");

                    b.Navigation("Shipping");

                    b.Navigation("SubmittedBy");

                    b.Navigation("Transaction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.PurchaseOrderDetail", b =>
                {
                    b.HasOne("Entities.Discount", "Discount")
                        .WithMany("PurchaseOrderDetails")
                        .HasForeignKey("DiscountId");

                    b.HasOne("Entities.Price", "Price")
                        .WithMany()
                        .HasForeignKey("PriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.PurchaseOrder", "PurchaseOrder")
                        .WithMany("PurchaseOrderDetails")
                        .HasForeignKey("PurchaseOrderId");

                    b.Navigation("Discount");

                    b.Navigation("Price");

                    b.Navigation("Product");

                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("Entities.SendInformation", b =>
                {
                    b.HasOne("Entities.City", "City")
                        .WithMany("SendInformation")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.State", "State")
                        .WithMany("SendInformation")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("City");

                    b.Navigation("State");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.SlideShow", b =>
                {
                    b.HasOne("Entities.Category", "Category")
                        .WithMany("SlideShows")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Entities.Product", "Product")
                        .WithMany("SlideShows")
                        .HasForeignKey("ProductId");

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Entities.Transaction", b =>
                {
                    b.HasOne("Entities.HolooCompany", "HolooCompany")
                        .WithMany()
                        .HasForeignKey("HolooCompanyId");

                    b.HasOne("Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("Transactions")
                        .HasForeignKey("PaymentMethodId");

                    b.HasOne("Entities.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId");

                    b.Navigation("HolooCompany");

                    b.Navigation("PaymentMethod");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.Unit", b =>
                {
                    b.HasOne("Entities.HolooCompany", "HolooCompany")
                        .WithMany()
                        .HasForeignKey("HolooCompanyId");

                    b.Navigation("HolooCompany");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.HasOne("Entities.City", "City")
                        .WithMany("User")
                        .HasForeignKey("CityId");

                    b.HasOne("Entities.HolooCompany", "HolooCompany")
                        .WithMany()
                        .HasForeignKey("HolooCompanyId");

                    b.HasOne("Entities.State", "State")
                        .WithMany("Users")
                        .HasForeignKey("StateId");

                    b.HasOne("Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("UserRoleId");

                    b.Navigation("City");

                    b.Navigation("HolooCompany");

                    b.Navigation("State");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("Entities.WishList", b =>
                {
                    b.HasOne("Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KeywordProduct", b =>
                {
                    b.HasOne("Entities.Keyword", null)
                        .WithMany()
                        .HasForeignKey("KeywordsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductProductAttributeGroup", b =>
                {
                    b.HasOne("Entities.ProductAttributeGroup", null)
                        .WithMany()
                        .HasForeignKey("AttributeGroupProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductTag", b =>
                {
                    b.HasOne("Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Blog", b =>
                {
                    b.Navigation("BlogComments");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Entities.BlogAuthor", b =>
                {
                    b.Navigation("Blogs");
                });

            modelBuilder.Entity("Entities.BlogCategory", b =>
                {
                    b.Navigation("BlogCategories");

                    b.Navigation("Blogs");
                });

            modelBuilder.Entity("Entities.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Entities.Category", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("SlideShows");
                });

            modelBuilder.Entity("Entities.City", b =>
                {
                    b.Navigation("SendInformation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.Color", b =>
                {
                    b.Navigation("Prices");
                });

            modelBuilder.Entity("Entities.Currency", b =>
                {
                    b.Navigation("Prices");
                });

            modelBuilder.Entity("Entities.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Entities.Discount", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Prices");

                    b.Navigation("PurchaseOrderDetails");

                    b.Navigation("PurchaseOrders");
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.Navigation("BlogComments");

                    b.Navigation("PurchaseOrderAccountant");

                    b.Navigation("PurchaseOrderApprovedBy");

                    b.Navigation("PurchaseOrderSubmittedBy");
                });

            modelBuilder.Entity("Entities.PaymentMethod", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Entities.Product", b =>
                {
                    b.Navigation("AttributeValues");

                    b.Navigation("Images");

                    b.Navigation("Prices");

                    b.Navigation("ProductComments");

                    b.Navigation("ProductUserRanks");

                    b.Navigation("SlideShows");
                });

            modelBuilder.Entity("Entities.ProductAttribute", b =>
                {
                    b.Navigation("AttributeValue");
                });

            modelBuilder.Entity("Entities.ProductAttributeGroup", b =>
                {
                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("Entities.PurchaseOrder", b =>
                {
                    b.Navigation("PurchaseOrderDetails");
                });

            modelBuilder.Entity("Entities.Shipping", b =>
                {
                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("Entities.Size", b =>
                {
                    b.Navigation("Prices");
                });

            modelBuilder.Entity("Entities.State", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("SendInformation");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Entities.Supplier", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Entities.Transaction", b =>
                {
                    b.Navigation("PurchaseOrders");
                });

            modelBuilder.Entity("Entities.Unit", b =>
                {
                    b.Navigation("Prices");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Navigation("BlogComments");

                    b.Navigation("PurchaseOrders");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
