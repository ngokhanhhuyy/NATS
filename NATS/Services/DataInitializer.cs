using NATS.Services.Entity;

namespace NATS.Services;

public sealed class DataInitializer
{
    private DatabaseContext _context;
    private UserManager<User> _userManager;
    private RoleManager<Role> _roleManager;
    
    public void InitializeData(IApplicationBuilder builder)
    {
        using (IServiceScope serviceScope = builder.ApplicationServices.CreateScope())
        {
            _context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
            _userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
            _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
            using (var transaction = _context.Database.BeginTransaction())
            {
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
                    InitializeBusinessServices();
                    InitializeHomePageSliderItems();
                    InitializePosts();
                    InitializeContactInfo();
                    InitializeTraffics();
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
                IdentityResult result = _roleManager.CreateAsync(role).GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);
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
            foreach (KeyValuePair<User, (string Password, string RoleName)> pair in users)
            {
                IdentityResult result = _userManager
                    .CreateAsync(pair.Key, pair.Value.Password)
                    .GetAwaiter()
                    .GetResult();
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);
                }
                result = _userManager
                    .AddToRoleAsync(pair.Key, pair.Value.RoleName)
                    .GetAwaiter()
                    .GetResult();
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);
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
                MainPhotoUrl = "/images/front-pages/about-us/1.jpg",
                MainQuoteContent = "Trong cuộc sống hiện đại, nhiều áp lực và lo lắng " +
                                    "khiến cho chúng ta càng ngày càng cảm thấy căng thảng, " +
                                    "mệt mỏi và có xu hướng tìm các giải pháp để cải thiện " +
                                    "sức khỏe và làm chậm quá trình lão hóa." +
                                    Environment.NewLine + Environment.NewLine +
                                    "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                    "hướng đến việc xây dựng văn hóa sức khỏe, thẩm mỹ " +
                                    "và lan tỏa giá trị tốt đẹp này đến với cộng đồng.",
                AboutUsContent = "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                "là đơn vị trực thuộc Viện Khoa học Giáo dục và Môi trường (IEES), " +
                                "ra đời với sứ mệnh giúp định hướng nghề nghiệp cho thế hệ trẻ. " +
                                "Đào tạo cho họ các kỹ thuật thẩm mỹ không xâm lấn theo phong thủy " +
                                "độc đáo, các kỹ thuật đả thông kinh lạc, các phương pháp thải độc " +
                                "và trẻ hóa tế bào. Chuyển giao quy trình mở spa dưỡng sinh trị liệu.",
                WhyChooseUsContent = "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                    "là đơn vị trực thuộc Viện Khoa học Giáo dục và Môi trường (IEES), " +
                                    "ra đời với sứ mệnh giúp định hướng nghề nghiệp cho thế hệ trẻ. " +
                                    "Đào tạo cho họ các kỹ thuật thẩm mỹ không xâm lấn theo phong thủy " +
                                    "độc đáo, các kỹ thuật đả thông kinh lạc, các phương pháp thải độc " +
                                    "và trẻ hóa tế bào. Chuyển giao quy trình mở spa dưỡng sinh trị liệu.",
                OurDifferenceContent = "Tại Trung Tâm NATS, bạn sẽ không chỉ được trang bị các kỹ năng " +
                                        "cần thiết để thành công trong ngành chăm sóc sức khỏe và " +
                                        "làm đẹp, mà còn được khuyến khích phát triển bản thân, " +
                                        "khám phá tài năng, sự tự tin và vẻ đẹp bên trong của chính mình." +
                                        Environment.NewLine + Environment.NewLine +
                                        "Đội ngũ chuyên gia giàu kinh nghiệm trong ngành Thẩm mỹ " +
                                        "và Chăm sóc sức khỏe nhiều năm qua tại Việt Nam đã sáng lập " +
                                        "Trung Tâm NATS với mong muốn tạo ra những học viên ưu tú, " +
                                        "được tôn trọng và công nhận trong ngành. NATS là một cộng đồng " +
                                        "học tập thân thiện và tương trợ lẫn nhau, nơi mỗi người có thể " +
                                        "khám phá tiềm năng, giá trị và hy vọng của bản thân cho tương lai. " +
                                        "Hãy đến với NATS để khám phá sự khác biệt của chúng tôi và trở " +
                                        "thành một trong những chuyên gia Thẩm mỹ và Chăm sóc sức khỏe " +
                                        "tốt nhất trong ngành!",
                OurCultureContent = "Với những mục tiêu cao cả trong việc đào tạo những chuyên gia " +
                                    "thẩm mỹ không xâm lấn và chăm sóc sức khỏe tốt nhất, tuy nhiên " +
                                    "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) cũng luôn " +
                                    "chú trọng tới văn hoá trong môi trường làm việc của ngành " +
                                    "thẩm mỹ và chăm sóc sức khoẻ." +
                                    Environment.NewLine + Environment.NewLine +
                                    "Chúng tôi tin rằng văn hoá là một phần không thể thiếu trong " +
                                    "sự phát triển bền vững của một tổ chức. Tại NATS, chúng tôi " +
                                    "tạo ra một môi trường làm việc tích cực và hỗ trợ cho đội ngũ " +
                                    "nhân viên và học viên. Chúng tôi khuyến khích sự sáng tạo và " +
                                    "đóng góp ý kiến, và xây dựng một cộng đồng thân thiện và đoàn kết."
            };
            _context.AboutUsIntroductions.Add(aboutUsIntroduction);
            _context.SaveChanges();
        }
    }

    private void InitializeTeamMembers()
    {
        if (!_context.TeamMembers.Any())
        {
            Faker faker = new Faker("vi");
            List<(string, string , string, string)> memberIntroductions;
            memberIntroductions = new List<(string, string, string, string)>
            {
                ("Đỗ Quang Huyền", "Giám đốc", null, "/images/front-pages/members/1.png"),
                (
                    "Trang Nguyễn",
                    "Phó giám đốc / Giảng viên",
                    "Kinh nghiệm 10 năm trong ngành chăm sóc sức khoẻ cộng đồng. " +
                    "Nhiều năm kinh nghiệm đào tạo về mỹ phẩm và sản phẩm chăm sóc sức khoẻ. " +
                    "Giảng viên Thần Số Học.",
                    "/images/front-pages/members/2.png"
                ),
                ("Lan Nguyễn", "Giám đốc chi nhánh Trà Vinh", null, "/images/front-pages/members/3.png"),
                ("Trần Kim Khoa", "Giám đốc chi nhánh Trà Vinh", null, "/images/front-pages/members/4.png")
            };
            foreach ((string, string, string, string) introduction in memberIntroductions)
            {
                TeamMember member = new TeamMember
                {
                    FullName = introduction.Item1,
                    RoleName = introduction.Item2,
                    Description = introduction.Item3 ?? faker.Lorem.Sentences(10),
                    PhotoUrl = introduction.Item4
                };
                _context.TeamMembers.Add(member);
            }

            _context.SaveChanges();
        }
    }

    private void InitializeBusinessCertificates()
    {
        if (!_context.BusinessCertificates.Any())
        {
            BusinessCertificate certificate = new BusinessCertificate
            {
                Name = "Quyết định Thành lập",
                PhotoUrl = "/images/front-pages/certificates/1.jpg"
            };
            _context.BusinessCertificates.Add(certificate);
            _context.SaveChangesAsync();
        }
    }

    private void InitializeIntroductionItems()
    {
        if (!_context.IntroductionItems.Any())
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
                IntroductionItem item = new IntroductionItem
                {
                    Name = pair.Key,
                    Summary = faker.Lorem.Paragraph(4),
                    Content = faker.Lorem.Paragraph(12) +
                            Environment.NewLine +
                            faker.Lorem.Paragraph(15),
                    ThumbnailUrl = pair.Value
                };
                _context.IntroductionItems.Add(item);
            }

            _context.SaveChanges();
        }
    }

    private void InitializeCourses()
    {
        if (!_context.Courses.Any())
        {
            Faker faker = new Faker("vi");
            Random random = new Random();
            // Initialize courses
            List<(string, string, string)> dataCourses = new List<(string, string, string)>
            {
                (
                    "Khóa Học Nghệ Thuật Trang Điểm Chuyên Nghiệp",
                    "Chương trình này cung cấp các kỹ năng cần thiết về trang điểm " +
                    "từ cơ bản đến nâng cao, giúp học viên trở thành chuyên gia " +
                    "trang điểm chuyên nghiệp.",
                    "/images/front-pages/courses/1.jpg"
                ),
                (
                    "Lớp Học Chăm Sóc Da Toàn Diện",
                    "Khóa học này tập trung vào việc chăm sóc và điều trị da, bao gồm " +
                    "các phương pháp làm sạch da, massage, và các liệu pháp chăm sóc da mặt chuyên sâu.",
                    "/images/front-pages/courses/2.jpg"
                ),
                (
                    "Chương Trình Đào Tạo Nghệ Thuật Làm Tóc",
                    "Dành cho những ai muốn trở thành nhà tạo mẫu tóc chuyên nghiệp, " +
                    "chương trình này bao gồm cắt, nhuộm, tạo kiểu tóc và các kỹ thuật làm tóc khác.",
                    "/images/front-pages/courses/3.jpg"
                ),
                (
                    "Khóa Học Nail Nghệ Thuật và Thiết Kế",
                    "Cung cấp kiến thức và kỹ năng từ cơ bản đến nâng cao trong lĩnh vực làm nail, " +
                    "bao gồm vẽ nail, phủ gel, và thiết kế nail nghệ thuật.",
                    "/images/front-pages/courses/4.jpg"
                ),
            };
            Dictionary<int, string[]> dataCoursePhotos = new Dictionary<int, string[]>
            {
                {
                    1,
                    new string[]
                    {
                        "/images/front-pages/courses/1_1.jpg",
                        "/images/front-pages/courses/1_2.jpg",
                        "/images/front-pages/courses/1_3.jpg",
                        "/images/front-pages/courses/1_4.jpg",
                        "/images/front-pages/courses/1_5.jpg",
                        "/images/front-pages/courses/1_6.jpg",
                    }
                },
                {
                    2,
                    new string[]
                    {
                        "/images/front-pages/courses/2_1.jpg",
                        "/images/front-pages/courses/2_2.jpg",
                        "/images/front-pages/courses/2_3.jpg",
                    }
                },
                {
                    3,
                    new string[]
                    {
                        "/images/front-pages/courses/3_1.jpg",
                    }
                },
            };
            for (int courseIndex = 0; courseIndex < dataCourses.Count; courseIndex++)
            {
                (string Name, string Summary, string ThumbnailUrl) dataCourse = dataCourses[courseIndex];
                Course course = new Course
                {
                    Name = dataCourse.Name,
                    Summary = dataCourse.Summary,
                    Detail = faker.Lorem.Paragraph(5) + Environment.NewLine +
                        faker.Lorem.Paragraph(8) + Environment.NewLine +
                        faker.Lorem.Paragraph(10),
                    ThumbnailUrl = dataCourse.ThumbnailUrl,
                    Sections = new List<CourseSection>(),
                    Photos = new List<CoursePhoto>()
                };
                _context.Add(course);


                // Initialize course sections
                for (int i = 0; i < random.Next(5, 10); i++)
                {
                    CourseSection courseSection = new CourseSection
                    {
                        Content = faker.Lorem.Sentence()
                    };
                    course.Sections.Add(courseSection);
                }

                // Initialize course photos
                dataCoursePhotos.TryGetValue(courseIndex + 1, out string[] dataCoursePhotoUrls);
                if (dataCoursePhotoUrls != null)
                {
                    foreach (string url in dataCoursePhotoUrls)
                    {
                        CoursePhoto photo = new CoursePhoto
                        {
                            Url = url,
                        };
                        course.Photos.Add(photo);
                    }
                }
            }

            _context.SaveChanges();
        }
    }

    private void InitializeBusinessServices()
    {
        if (!_context.BusinessServices.Any())
        {
            Faker faker = new Faker("vi");
            Random random = new Random();
            // Initialize business services
            List<(string, string, string)> dataServices = new List<(string, string, string)>
            {
                (
                    "Dịch vụ massage toàn thân",
                    "Dùng các kỹ thuật massage truyền thống kết hợp với tinh dầu tự nhiên để thư giãn " +
                    "cơ bắp, giảm stress và cải thiện lưu thông máu.",
                    "/images/front-pages/services/1.jpg"
                ),
                (
                    "Liệu pháp da mặt chống lão hóa",
                    "Sử dụng các sản phẩm chăm sóc da cao cấp và công nghệ tiên tiến để giảm thiểu các " +
                    "dấu hiệu lão hóa, làm mờ nếp nhăn, và tái tạo làn da.",
                    "/images/front-pages/services/2.jpg"
                ),
                (
                    "Dịch vụ tắm trắng toàn thân",
                    "Kết hợp giữa tắm hơi và sử dụng hỗn hợp tinh chất tự nhiên giúp làm sáng da, mờ " +
                    "vết thâm và cung cấp dưỡng chất.",
                    "/images/front-pages/services/3.jpg"
                ),
                (
                    "Dịch vụ chăm sóc móng tay/móng chân",
                    "Cung cấp dịch vụ làm sạch, tạo hình, và sơn móng chuyên nghiệp, kèm theo liệu pháp " +
                    "dưỡng ẩm cho da tay/da chân và massage nhẹ nhàng.",
                    "/images/front-pages/services/4.jpg"
                )
            };
            Dictionary<int, string[]> dataServicePhotos = new Dictionary<int, string[]>
            {
                {
                    1,
                    new string[]
                    {
                        "/images/front-pages/services/1_1.jpg",
                        "/images/front-pages/services/1_2.jpg",
                        "/images/front-pages/services/1_3.jpg",
                        "/images/front-pages/services/1_4.jpg",
                        "/images/front-pages/services/1_5.jpg",
                        "/images/front-pages/services/1_6.jpg",
                    }
                },
                {
                    2,
                    new string[]
                    {
                        "/images/front-pages/services/2_1.jpg",
                        "/images/front-pages/services/2_2.jpg",
                        "/images/front-pages/services/2_3.jpg",
                    }
                },
                {
                    3,
                    new string[]
                    {
                        "/images/front-pages/services/3_1.jpg",
                    }
                },
            };
            for (int serviceIndex = 0; serviceIndex < dataServices.Count; serviceIndex++)
            {
                (string Title, string Summary, string ThumbnailUrl) dataService;
                dataService = dataServices[serviceIndex];
                BusinessService service = new BusinessService
                {
                    Name = dataService.Title,
                    Summary = dataService.Summary,
                    Detail = faker.Lorem.Paragraph(5) + Environment.NewLine +
                        faker.Lorem.Paragraph(8) + Environment.NewLine +
                        faker.Lorem.Paragraph(10),
                    ThumbnailUrl = dataService.ThumbnailUrl,
                    Features = new List<BusinessServiceFeature>(),
                    Photos = new List<BusinessServicePhoto>()
                };
                _context.BusinessServices.Add(service);

                // Initialize business service features
                for (int i = 0; i < random.Next(5, 10); i++)
                {
                    BusinessServiceFeature serviceFeature = new BusinessServiceFeature
                    {
                        Content = faker.Lorem.Sentence()
                    };
                    service.Features.Add(serviceFeature);
                }

                // Initialize business service photos
                dataServicePhotos.TryGetValue(serviceIndex + 1, out string [] dataServicePhotoUrls);
                if (dataServicePhotoUrls != null)
                {
                    foreach (string url in dataServicePhotoUrls)
                    {
                        BusinessServicePhoto servicePhoto = new BusinessServicePhoto
                        {
                            Url = url
                        };
                        service.Photos.Add(servicePhoto);
                    }
                }
            }

            _context.SaveChanges();
        }
    }

    private void InitializeHomePageSliderItems()
    {
        if (!_context.HomePageSliderItems.Any())
        {
            string[] photoUrls = new string[]
            {
                "/images/front-pages/homepage-slider-items/1.jpg",
                "/images/front-pages/homepage-slider-items/2.jpg",
                "/images/front-pages/homepage-slider-items/3.jpg"
            };

            for (int i = 0; i < photoUrls.Count(); i++)
            {
                HomePageSliderItem item = new HomePageSliderItem
                {
                    PhotoUrl = photoUrls[i],
                    Index = i
                };
                _context.HomePageSliderItems.Add(item);
            }
            _context.SaveChanges();
        }
    }
    
    private void InitializePosts()
    {
        // Initialize posts
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
                    NormalizedTitle = title
                        .ToNonDiacritics()
                        .ToLower()
                        .Replace(" ", "-")
                        .Replace(".", "")
                        .Replace(",", "")
                        .Replace("đ", "d"),
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
                post.NormalizedTitle = post.NormalizedTitle.Replace(".", "").Replace(",", "").ToLower();
            }
            _context.SaveChanges();
        }
    }
    
    private void InitializeContactInfo()
    {
        if (!_context.ContactInfos.Any())
        {
            ContactInfo contactInfo = new ContactInfo
            {
                PhoneNumber = "0914 64 0979",
                ZaloNumber = "0914 64 0979",
                Email = "thammyquocgia@gmail.com",
                Address = "21 Phan Đăng Lưu, phường Tân An, thành phố Buôn Ma Thuột, tỉnh Đắk Lắk"
            };
            _context.ContactInfos.Add(contactInfo);
        }
        _context.SaveChanges();
    }
    
    private void InitializeTraffics()
    {
        // Generate traffic by hour entities up to 7 days ahead from today if the last entity
        // just covers up to less than 3 days ahead from today.
        TrafficByDate lastTrafficByDate = _context.TrafficByDates
            .Include(td => td.TrafficByHours)
            .OrderByDescending(td => td.RecordedAt)
            .Take(1)
            .FirstOrDefault();

        if (lastTrafficByDate == null || lastTrafficByDate.RecordedAt.Date < DateTime.Today.AddDays(3))
        {
            DateTime startingDateTime = lastTrafficByDate?.RecordedAt.AddDays(1) ?? DateTime.Today;
            DateTime generatingDateTime = startingDateTime;
            while (generatingDateTime <= DateTime.Today.AddDays(7))
            {
                TrafficByDate trafficByDate = new TrafficByDate
                {
                    RecordedAt = generatingDateTime,
                    TrafficByHours = new List<TrafficByHour>() 
                };
                _context.TrafficByDates.Add(trafficByDate);
                
                DateTime generatingTime = generatingDateTime;
                while (generatingTime < generatingDateTime.AddDays(1))
                {
                    TrafficByHour trafficByHour = new TrafficByHour
                    {
                        RecordedAt = generatingTime
                    };
                    trafficByDate.TrafficByHours.Add(trafficByHour);
                    generatingTime = generatingTime.AddHours(1);
                }
                generatingDateTime = generatingDateTime.AddDays(1);
            }
            
            _context.SaveChanges();
        }
    }
}