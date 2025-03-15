using System.Text.RegularExpressions;

namespace NATS.Services;

public sealed partial class DataInitializer
{
    private DatabaseContext _context;
    private UserManager<User> _userManager;
    private RoleManager<Role> _roleManager;
    
    public void InitializeData(IApplicationBuilder builder)
    {
        using IServiceScope serviceScope = builder.ApplicationServices.CreateScope();

        _context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
        _userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
        _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        using IDbContextTransaction transaction = _context.Database.BeginTransaction();

        try
        {
            InitializeRoles();
            InitializeUsers();
            IntializeAboutUsIntroduction();
            InitializeTeamMembers();
            InitializeGeneralSettings();
            InitializeBusinessCertificates();
            InitializeIntroductionItems();
            InitializeCourses();
            InitializeServices();
            InitializeSliderItems();
            InitializePosts();
            InitializeContactInfo();

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            _context.Database.CloseConnection();
        }
    }

    private void InitializeRoles()
    {
        if (!_roleManager.Roles.Any())
        {
            List<Role> roles = new List<Role>
            {
                new Role
                {
                    Name = "Developer",
                    DisplayName = "Nhà phát triển",
                },
                new Role
                {
                    Name = "Admin",
                    DisplayName = "Quản trị viên",
                },
                new Role
                {
                    Name = "ContentCreator",
                    DisplayName = "Sáng tạo nội dung",
                },
            };

            foreach (Role role in roles)
            {
                IdentityResult result = _roleManager
                    .CreateAsync(role)
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded)
                {
                    string description = result.Errors.FirstOrDefault()?.Description;
                    throw new InvalidOperationException(description);
                }

                _context.SaveChanges();
            }
        }
    }

    private void InitializeUsers()
    {
        if (!_userManager.Users.Any())
        {
            Dictionary<User, (string Password, string RoleName)> users;
            users = new Dictionary<User, (string Password, string RoleName)>
            {
                {
                    new User
                    {
                        UserName = "ngokhanhhuyy",
                    },
                    ("Huyy47b1", "Developer")
                },
                {
                    new User
                    {
                        UserName = "thuytrangnguyen",
                    },
                    ("trang123", "Admin")
                },
                {
                    new User
                    {
                        UserName = "anhtaingo",
                    },
                    ("tai123", "ContentCreator")
                }
            };

            string description;
            foreach (KeyValuePair<User, (string Password, string RoleName)> pair in users)
            {
                IdentityResult result = _userManager
                    .CreateAsync(pair.Key, pair.Value.Password)
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded)
                {
                    description = result.Errors.FirstOrDefault()?.Description;
                    throw new InvalidOperationException(description);
                }

                result = _userManager
                    .AddToRoleAsync(pair.Key, pair.Value.RoleName)
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded)
                {
                    description = result.Errors.FirstOrDefault()?.Description;
                    throw new InvalidOperationException();
                }
            }
        }

        _context.SaveChanges();
    }

    private void InitializeGeneralSettings()
    {
        if (!_context.GeneralSettings.Any())
        {
            GeneralSettings settings = new GeneralSettings
            {
                ApplicationName = "Trung tâm Khoa học Đào tạo và Thẩm mỹ Quốc Gia",
                ApplicationShortName = "NATS",
                FavIconUrl = "/images/favicon.ico"
            };

            _context.GeneralSettings.Add(settings);
            _context.SaveChanges();
        }
    }

    private void IntializeAboutUsIntroduction()
    {
        if (!_context.AboutUsIntroductions.Any())
        {
            AboutUsIntroduction aboutUsIntroduction = new AboutUsIntroduction
            {
                ThumbnailUrl = "/images/front-pages/about-us/1.jpg",
                MainQuoteContent = "Trong cuộc sống hiện đại, nhiều áp lực và lo lắng " +
                                    "khiến cho chúng ta càng ngày càng cảm thấy căng thảng, " +
                                    "mệt mỏi và có xu hướng tìm các giải pháp để cải thiện " +
                                    "sức khỏe và làm chậm quá trình lão hóa." +
                                    Environment.NewLine + Environment.NewLine +
                                    "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                    "hướng đến việc xây dựng văn hóa sức khỏe, thẩm mỹ " +
                                    "và lan tỏa giá trị tốt đẹp này đến với cộng đồng.",
                AboutUsContent = "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                "là đơn vị trực thuộc Viện Khoa học Giáo dục và Môi trường " +
                                "(IEES), ra đời với sứ mệnh giúp định hướng nghề nghiệp cho " +
                                "thế hệ trẻ. Đào tạo cho họ các kỹ thuật thẩm mỹ không xâm " +
                                "lấn theo phong thủy độc đáo, các kỹ thuật đả thông kinh " +
                                "lạc, các phương pháp thải độc và trẻ hóa tế bào. Chuyển " +
                                "giao quy trình mở spa dưỡng sinh trị liệu.",
                WhyChooseUsContent = "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                    "là đơn vị trực thuộc Viện Khoa học Giáo dục và Môi " +
                                    "trường (IEES), ra đời với sứ mệnh giúp định hướng nghề " +
                                    "nghiệp cho thế hệ trẻ. Đào tạo cho họ các kỹ thuật " +
                                    "thẩm mỹ không xâm lấn theo phong thủy độc đáo, các kỹ " +
                                    "thuật đả thông kinh lạc, các phương pháp thải độc và " +
                                    "trẻ hóa tế bào. Chuyển giao quy trình mở spa dưỡng " +
                                    "sinh trị liệu.",
                OurDifferenceContent = "Tại Trung Tâm NATS, bạn sẽ không chỉ được trang bị " +
                                        "các kỹ năng cần thiết để thành công trong ngành " +
                                        "chăm sóc sức khỏe và làm đẹp, mà còn được khuyến " +
                                        "khích phát triển bản thân, khám phá tài năng, sự " +
                                        "tự tin và vẻ đẹp bên trong của chính mình." +
                                        Environment.NewLine + Environment.NewLine +
                                        "Đội ngũ chuyên gia giàu kinh nghiệm trong ngành " +
                                        "Thẩm mỹ và Chăm sóc sức khỏe nhiều năm qua tại " +
                                        "Việt Nam đã sáng lập Trung Tâm NATS với mong muốn " +
                                        "tạo ra những học viên ưu tú, được tôn trọng và " +
                                        "công nhận trong ngành. NATS là một cộng đồng học " +
                                        "tập thân thiện và tương trợ lẫn nhau, nơi mỗi " +
                                        "người có thể khám phá tiềm năng, giá trị và hy " +
                                        "vọng của bản thân cho tương lai. Hãy đến với NATS " +
                                        "để khám phá sự khác biệt của chúng tôi và trở " +
                                        "thành một trong những chuyên gia Thẩm mỹ và Chăm " +
                                        "sóc sức khỏe tốt nhất trong ngành!",
                OurCultureContent = "Tuy ới những mục tiêu cao cả trong việc đào tạo những " +
                                    "chuyên gia thẩm mỹ không xâm lấn và chăm sóc sức khỏe " +
                                    "tốt nhất, Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc " +
                                    "Gia (NATS) cũng luôn chú trọng tới văn hoá trong môi " +
                                    "trường làm việc của ngành thẩm mỹ và chăm sóc sức khoẻ." +
                                    Environment.NewLine + Environment.NewLine +
                                    "Chúng tôi tin rằng văn hoá là một phần không thể thiếu " +
                                    "trong sự phát triển bền vững của một tổ chức. Tại " +
                                    "NATS, chúng tôi tạo ra một môi trường làm việc tích " +
                                    "cực và hỗ trợ cho đội ngũ nhân viên và học viên. Chúng " +
                                    "tôi khuyến khích sự sáng tạo và đóng góp ý kiến, xây " +
                                    "dựng một cộng đồng thân thiện và đoàn kết."
            };

            _context.AboutUsIntroductions.Add(aboutUsIntroduction);
            _context.SaveChanges();
        }
    }

    private void InitializeTeamMembers()
    {
        if (!_context.Members.Any())
        {
            Faker faker = new Faker("vi");
            List<Member> members = new List<Member>
            {
                new Member
                {
                    FullName = "Đỗ Quang Huyền",
                    RoleName = "Giám đốc",
                    Description = faker.Lorem.Sentences(10),
                    ThumbnailUrl = "/images/front-pages/members/1.png"
                },
                new Member
                {
                    FullName = "Trang Nguyễn",
                    RoleName = "Phó giám đốc / Giảng viên",
                    Description = "Kinh nghiệm 10 năm trong ngành chăm sóc sức khoẻ cộng " +
                                "đồng. Nhiều năm kinh nghiệm đào tạo về mỹ phẩm và sản phẩm " +
                                "chăm sóc sức khoẻ. Giảng viên Thần Số Học.",
                    ThumbnailUrl = "/images/front-pages/members/2.png"
                },
                new Member
                {
                    FullName = "Lan Nguyễn",
                    RoleName = "Giám đốc chi nhánh Trà Vinh",
                    Description = faker.Lorem.Sentences(10),
                    ThumbnailUrl = "/images/front-pages/members/3.png"
                },
                new Member
                {
                    FullName = "Trần Kim Khoa",
                    RoleName = "Giám đốc chi nhánh Trà Vinh",
                    Description = faker.Lorem.Sentences(10),
                    ThumbnailUrl = "/images/front-pages/members/4.png"
                },
            };

            _context.Members.AddRange(members);
            _context.SaveChanges();
        }
    }

    private void InitializeBusinessCertificates()
    {
        if (!_context.Certificates.Any())
        {
            Certificate certificate = new Certificate
            {
                Name = "Quyết định Thành lập",
                ThumbnailUrl = "/images/front-pages/certificates/1.jpg"
            };

            _context.Certificates.Add(certificate);
            _context.SaveChanges();
        }
    }

    private void InitializeIntroductionItems()
    {
        if (!_context.SummaryItems.Any())
        {
            Faker faker = new Faker("vi");
            Dictionary<string, string> dataItems = new Dictionary<string, string>
            {
                {
                    "Thẩm mỹ"  + Environment.NewLine + "cột sống",
                    "/images/front-pages/introduction-items/5.jpg"
                },
                {
                    "Đả thông" + Environment.NewLine + "kinh lạc",
                    "/images/front-pages/introduction-items/6.jpg"
                },
                {
                    "Thải độc" + Environment.NewLine + "tế bào",
                    "/images/front-pages/introduction-items/7.jpg"
                },
                {
                    "Nhân số học &" + Environment.NewLine + "Thiền",
                    "/images/front-pages/introduction-items/8.jpg"
                }
            };
            foreach (KeyValuePair<string, string> pair in dataItems)
            {
                SummaryItem item = new SummaryItem
                {
                    Name = pair.Key,
                    SummaryContent = faker.Lorem.Paragraph(4),
                    DetailContent = faker.Lorem.Paragraph(12) +
                            Environment.NewLine +
                            faker.Lorem.Paragraph(15),
                    ThumbnailUrl = pair.Value
                };
                _context.SummaryItems.Add(item);
            }

            _context.SaveChanges();
        }
    }

    private void InitializeCourses()
    {
        if (!_context.CatalogItems.Any())
        {
            Faker faker = new Faker("vi");
            List<CatalogItem> courses = new List<CatalogItem>
            {
                new CatalogItem
                {
                    Name = "Khóa Học Nghệ Thuật Trang Điểm Chuyên Nghiệp",
                    Summary = "Khóa học này tập trung vào việc chăm sóc và điều trị da, bao " +
                            "gồm các phương pháp làm sạch da, massage, và các liệu pháp " +
                            "chăm sóc da mặt chuyên sâu.",
                    ThumbnailUrl = "/images/front-pages/courses/1.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/1_1.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/1_2.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/1_3.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/1_4.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/1_5.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/1_6.jpg" },
                    } 
                },
                new CatalogItem
                {
                    Name = "Lớp Học Chăm Sóc Da Toàn Diện",
                    Summary = "Chương trình này cung cấp các kỹ năng cần thiết về trang " +
                            "điểm từ cơ bản đến nâng cao, giúp học viên trở thành chuyên " +
                            "gia trang điểm chuyên nghiệp.",
                    ThumbnailUrl = "/images/front-pages/courses/2.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/2_1.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/2_2.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/2_3.jpg" }
                    } 
                },
                new CatalogItem
                {
                    Name = "Chương Trình Đào Tạo Nghệ Thuật Làm Tóc",
                    Summary = "Dành cho những ai muốn trở thành nhà tạo mẫu tóc chuyên " +
                            "nghiệp, chương trình này bao gồm cắt, nhuộm, tạo kiểu tóc và " +
                            "các kỹ thuật làm tóc khác.",
                    ThumbnailUrl = "/images/front-pages/courses/3.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/courses/3_1.jpg" },
                    } 
                },
                new CatalogItem
                {
                    Name = "Khóa Học Nail Nghệ Thuật và Thiết Kế",
                    Summary = "Cung cấp kiến thức và kỹ năng từ cơ bản đến nâng cao trong " +
                            "lĩnh vực làm nail, bao gồm vẽ nail, phủ gel, và thiết kế nail " +
                            "nghệ thuật.",
                    ThumbnailUrl = "/images/front-pages/courses/4.jpg",
                },
            };

            foreach (CatalogItem course in courses)
            {
                course.Detail = faker.Lorem.Paragraph(5) + Environment.NewLine +
                                faker.Lorem.Paragraph(8) + Environment.NewLine +
                                faker.Lorem.Paragraph(10);
                _context.Add(course);
            }

            _context.SaveChanges();
        }
    }

    private void InitializeServices()
    {
        if (!_context.CatalogItems.Any())
        {
            Faker faker = new Faker("vi");

            List<CatalogItem> services = new List<CatalogItem>
            {
                new CatalogItem
                {
                    Name = "Dịch vụ massage toàn thân",
                    Summary = "Dùng các kỹ thuật massage truyền thống kết hợp với tinh dầu " +
                            "tự nhiên để thư giãn cơ bắp, giảm stress và cải thiện lưu " +
                            "thông máu.",
                    ThumbnailUrl = "/images/front-pages/services/1.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_1.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_2.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_3.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_4.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_5.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_6.jpg" },
                    } 
                },
                new CatalogItem
                {
                    Name = "Liệu pháp da mặt chống lão hóa",
                    Summary = "Sử dụng các sản phẩm chăm sóc da cao cấp và công nghệ tiên " +
                            "tiến để giảm thiểu các dấu hiệu lão hóa, làm mờ nếp nhăn, và " +
                            "tái tạo làn da.",
                    ThumbnailUrl = "/images/front-pages/services/2.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/services/2_1.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/2_2.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/2_3.jpg" }
                    } 
                },
                new CatalogItem
                {
                    Name = "Dịch vụ tắm trắng toàn thân",
                    Summary = "Kết hợp giữa tắm hơi và sử dụng hỗn hợp tinh chất tự nhiên " +
                            "giúp làm sáng da, mờ vết thâm và cung cấp dưỡng chất.",
                    ThumbnailUrl = "/images/front-pages/services/3.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/services/3_1.jpg" },
                    } 
                },
                new CatalogItem
                {
                    Name = "Dịch vụ chăm sóc móng tay/móng chân",
                    Summary = "Cung cấp dịch vụ làm sạch, tạo hình, và sơn móng chuyên " +
                            "nghiệp, kèm theo liệu pháp dưỡng ẩm cho da tay/da chân và " +
                            "massage nhẹ nhàng.",
                    ThumbnailUrl = "/images/front-pages/services/4.jpg",
                },
            };

            foreach (CatalogItem service in services)
            {
                service.Detail = faker.Lorem.Paragraph(5) + Environment.NewLine +
                                faker.Lorem.Paragraph(8) + Environment.NewLine +
                                faker.Lorem.Paragraph(10);
                _context.Add(service);
            }

            _context.SaveChanges();
        }
    }

    private void InitializeSliderItems()
    {
        if (!_context.SliderItems.Any())
        {
            string[] photoUrls = new string[]
            {
                "/images/front-pages/slider-items/1.jpg",
                "/images/front-pages/slider-items/2.jpg",
                "/images/front-pages/slider-items/3.jpg"
            };

            for (int i = 0; i < photoUrls.Length; i++)
            {
                SliderItem item = new SliderItem
                {
                    ThumbnailUrl = photoUrls[i],
                    Index = i
                };

                _context.SliderItems.Add(item);
            }

            _context.SaveChanges();
        }
    }
    
    private void InitializePosts()
    {
        // Initialize posts.
        if (!_context.Posts.Any())
        {
            Faker faker = new Faker("vi");
            Random random = new Random();
            for (int i = 0; i < 30; i++)
            {
                string title = faker.Lorem.Sentence(20);
                Post post = new Post
                {
                    Title = title,
                    NormalizedTitle = NormalizedTitleProhibitedCharactersRegex()
                        .Replace(
                            title.ToNonDiacritics()
                                .ToLower()
                                .Replace(" ", "-")
                                .Replace("đ", "d"),
                            ""),
                    Content = faker.Lorem.Paragraphs(random.Next(15, 20), Environment.NewLine),
                    UserId = _context.Users
                        .Where(u => u.UserName == "ngokhanhhuyy")
                        .Select(u => u.Id)
                        .Single(),
                };
                _context.Posts.Add(post);
                _context.SaveChanges();
            }
        }
        else
        {
            List<Post> posts = _context.Posts.ToList();
            foreach (Post post in posts)
            {
                post.NormalizedTitle = post.NormalizedTitle
                    .Replace(".", "")
                    .Replace(",", "")
                    .ToLower();
            }

            _context.SaveChanges();
        }
    }
    
    private void InitializeContactInfo()
    {
        if (!_context.Contacts.Any())
        {
            List<Contact> contacts = new List<Contact>
            {
                new Contact
                {
                    Type = ContactType.PhoneNumber,
                    Content = "0914 64 0979",
                },
                new Contact
                {
                    Type = ContactType.ZaloNumber,
                    Content = "0914 64 0979",
                },
                new Contact
                {
                    Type = ContactType.Email,
                    Content = "thammyquocgia@gmail.com",
                },
                new Contact
                {
                    Type = ContactType.Address,
                    Content = "21 Phan Đăng Lưu, phường Tân An, " +
                            "thành phố Buôn Ma Thuột, tỉnh Đắk Lắk",
                },
            };

            _context.Contacts.AddRange(contacts);
        }

        _context.SaveChanges();
    }
    
    [GeneratedRegex(@"[.,\?\-:;/><\(\)]")]
    private static partial Regex NormalizedTitleProhibitedCharactersRegex();
}