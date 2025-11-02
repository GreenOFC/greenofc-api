using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class LeadPtfSeedData
    {
        public static IReadOnlyList<DataConfig> Genders = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Gender, GreenType = GreenType.GreenP, Key = "M", Value = "Nam" },
            new DataConfig{ Type = DataConfigType.Gender, GreenType = GreenType.GreenP, Key = "F", Value = "Nữ" },
        };

        public static IReadOnlyList<DataConfig> MaritalStatus = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenP, Key = "D", Value = "Ly hôn" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenP, Key = "F", Value = "Sống với gia đình" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenP, Key = "M", Value = "Đã lập gia đình" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenP, Key = "S", Value = "Ly thân" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenP, Key = "A", Value = "Độc thân" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenP, Key = "W", Value = "Góa phụ" }
        };

        public static IReadOnlyList<DataConfig> DependentTypes = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.DependentType, GreenType = GreenType.GreenP, Key = "Couple", Value = "Vợ, chồng" },
            new DataConfig{ Type = DataConfigType.DependentType, GreenType = GreenType.GreenP, Key = "Children", Value = "Trẻ em" },
            new DataConfig{ Type = DataConfigType.DependentType, GreenType = GreenType.GreenP, Key = "Parents", Value = "Bố mẹ" },
            new DataConfig{ Type = DataConfigType.DependentType, GreenType = GreenType.GreenP, Key = "Other", Value = "Khác" },
        };

        public static IReadOnlyList<DataConfig> EducationLevels = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenP, Key = "TH", Value = "Cấp I" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenP, Key = "THCS", Value = "Cấp II" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenP, Key = "HighSchool", Value = "Cấp III" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenP, Key = "Intermediate", Value = "Trung cấp" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenP, Key = "University", Value = "Đại học" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenP, Key = "Colleage", Value = "Cao đẳng" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenP, Key = "Postgraduate", Value = "Trên đại học" },
        };

        public static IReadOnlyList<DataConfig> Positions = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "BA", Value = "Quản trị kinh doanh" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "Engineer", Value = "Kỹ sư" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "Farmer", Value = "Nông dân" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "Sell", Value = "Bán hàng" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "Service", Value = "Dịch vụ" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "Worker", Value = "Công nhân" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "IT", Value = "Công nghệ thông tin" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "FinanceBanking", Value = "Tài chính & Ngân hàng" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "Freelance", Value = "Làm nghề tự do" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "RestaurantOrHotelOwner", Value = "Tự chủ: Nhà hàng, khách sạn" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "AutonomyEntertainmentField", Value = "Tự chủ: Lĩnh vực giải trí" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "AutonomyShopOwner", Value = "Tự chủ: Chủ cửa hàng" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "AutonomyExpert", Value = "Tự chủ: Chuyên gia" },
            new DataConfig{ Type = DataConfigType.Position, GreenType = GreenType.GreenP, Key = "Other", Value = "Khác" },
        };

        public static IReadOnlyList<DataConfig> JobTypes = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.JobType, GreenType = GreenType.GreenP, Key = "Staff", Value = "Nhân viên" },
            new DataConfig{ Type = DataConfigType.JobType, GreenType = GreenType.GreenP, Key = "Housewife", Value = "Nội trợ" },
            new DataConfig{ Type = DataConfigType.JobType, GreenType = GreenType.GreenP, Key = "Retirement", Value = "Nghỉ hưu" },
            new DataConfig{ Type = DataConfigType.JobType, GreenType = GreenType.GreenP, Key = "Self-employed", Value = "Tự kinh doanh" },
            new DataConfig{ Type = DataConfigType.JobType, GreenType = GreenType.GreenP, Key = "Student", Value = "Học sinh" },
            new DataConfig{ Type = DataConfigType.JobType, GreenType = GreenType.GreenP, Key = "Unemployment", Value = "Thất nghiệp" },
        };

        public static IReadOnlyList<DataConfig> SocialAccounts = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.SocialAccount, GreenType = GreenType.GreenP, Key = "Facebook", Value = "Facebook" },
            new DataConfig{ Type = DataConfigType.SocialAccount, GreenType = GreenType.GreenP, Key = "Zalo", Value = "Zalo" },
            new DataConfig{ Type = DataConfigType.SocialAccount, GreenType = GreenType.GreenP, Key = "Viber", Value = "Viber" },
            new DataConfig{ Type = DataConfigType.SocialAccount, GreenType = GreenType.GreenP, Key = "Other", Value = "Khác" },
        };

        public static IReadOnlyList<DataConfig> Relationships = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Couple", Value = "Vợ/Chồng" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Children", Value = "Con cái" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Parents", Value = "Bố/Mẹ" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Siblings", Value = "Anh/Em ruột" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Sisters", Value = "Chị/Em ruột" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "CousinA", Value = "Anh/Em họ" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "CousinB", Value = "Chị/Em họ" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Partner", Value = "Đồng nghiệp" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Friend", Value = "Bạn bè" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenP, Key = "Other", Value = "Khác" },
        };

        public static IReadOnlyList<DataConfig> LoanPurposes = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "BuyVehicle", Value = "Mua phương tiện vận chuyển (xe máy, xe đạp, …)" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "BuyHomeAppliances", Value = "Mua trang thiết bị gia đình (trang thiết bị, đồ điện tử, …)" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "BuyConsumables", Value = "Mua hàng tiêu dùng (đồ ăn, thức uống, quần áo,…)" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "EducationExpenses", Value = "Chi phí giáo dục" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "MedicalTreatmentCosts", Value = "Chi phí điều trị y tế" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "TravelExpenses", Value = "Chi phí du lịch" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "CostOfOrganizingEeddingsFunerals", Value = "Chi phí tổ chức đám cưới, tang lễ,…" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "BeautyCosts", Value = "Chi phí chăm sóc sức khỏe và làm đẹp" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "HousingRepairCosts", Value = "Chi phí sửa chữa nhà ở" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenP, Key = "Other", Value = "Khác" },
        };

        public static IReadOnlyList<DataConfig> Terms = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "5", Value = "5" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "6", Value = "6" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "11", Value = "11" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "12", Value = "12" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "17", Value = "17" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "18", Value = "18" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "23", Value = "23" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "24", Value = "24" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "29", Value = "29" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "30", Value = "30" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "35", Value = "35" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenP, Key = "36", Value = "36" },
        };

        public static IReadOnlyList<DataConfig> LoanServices = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.LoanService, GreenType = GreenType.GreenP, Key = "PTI", Value = "PTI" },
            new DataConfig{ Type = DataConfigType.LoanService, GreenType = GreenType.GreenP, Key = "VNI", Value = "VNI" },
        };

        public static IReadOnlyList<DataConfig> DisbursementMethods = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.DisbursementMethod, GreenType = GreenType.GreenP, Key = "SeabankNewAccount", Value = "Seabank new account" },
            new DataConfig{ Type = DataConfigType.DisbursementMethod, GreenType = GreenType.GreenP, Key = "OtherBankAccount", Value = "Other bank account" },
            new DataConfig{ Type = DataConfigType.DisbursementMethod, GreenType = GreenType.GreenP, Key = "Partners", Value = "Partners" },
        };

        public static IReadOnlyList<DataConfig> PartnerNames = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.PartnerName, GreenType = GreenType.GreenP, Key = "TrustSocial", Value = "Trust Social" },
            new DataConfig{ Type = DataConfigType.PartnerName, GreenType = GreenType.GreenP, Key = "Payoo", Value = "Payoo" },
            new DataConfig{ Type = DataConfigType.PartnerName, GreenType = GreenType.GreenP, Key = "Other", Value = "Other" },
        };

        public static IReadOnlyList<LeadPtfCategoryGroup> CategoryGroups = new LeadPtfCategoryGroup[]
        {
            new LeadPtfCategoryGroup {
                ProductLine = ProductLineEnum.TSA,
                Categories = new List<LeadPtfCategory>
                {
                    new LeadPtfCategory
                    {
                        Code = "CreditContract",
                        Name = "VAY THEO HỢP ĐỒNG TÍN DỤNG",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "CFA",
                                Name = "Cashloan CF A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "CFB",
                                Name = "Cashloan CF B"
                            },
                            new LeadPtfProduct
                            {
                                Code = "CFC",
                                Name = "Cashloan CF C"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "ElectricityBill",
                        Name = "VAY THEO HÓA ĐƠN ĐIỆN",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "EBA",
                                Name = "Cashloan EB A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "EBB",
                                Name = "Cashloan EB B"
                            },
                            new LeadPtfProduct
                            {
                                Code = "EBC",
                                Name = "Cashloan EB C"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "InsuranceContract",
                        Name = "VAY THEO HĐ BẢO HIỂM NHÂN THỌ",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "LIA",
                                Name = "Cashloan LI A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "LIB",
                                Name = "Cashloan LI B"
                            },
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "HealthInsuranceCard",
                        Name = "VAY THEO THẺ BHYT",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "BHYT",
                                Name = "Cashloan BHYT FAST"
                            },
                        }
                    },
                    new LeadPtfCategory
                    {
                       Code = "SIM",
                       Name = "VAY THEO SIM CHÍNH CHỦ",
                       Products = new List<LeadPtfProduct>
                       {
                           new LeadPtfProduct
                           {
                               Code = "Cashloan LG 39",
                               Name = "Cashloan LG 39"
                           },
                           new LeadPtfProduct
                           {
                               Code = "Cashloan LG 45",
                               Name = "Cashloan LG 45"
                           },
                           new LeadPtfProduct
                           {
                               Code = "Cashloan SCO 39",
                               Name = "Cashloan SCO 39"
                           },
                           new LeadPtfProduct
                           {
                               Code = "Cashloan SCO 45",
                               Name = "Cashloan SCO 45"
                           },
                       }
                    },
                    new LeadPtfCategory
                    {
                       Code = "DIENMAY",
                       Name = "VAY THEO HĐ TRẢ GÓP ĐIỆN MÁY",
                       Products = new List<LeadPtfProduct>
                       {
                           new LeadPtfProduct
                           {
                               Code = "CashloanCDA",
                               Name = "Cashloan CD A"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanCDB",
                               Name = "Cashloan CD B"
                           }
                       }
                    },
                    new LeadPtfCategory
                    {
                       Code = "NONSCORE",
                       Name = "NON - SCORE",
                       Products = new List<LeadPtfProduct>
                       {
                           new LeadPtfProduct
                           {
                               Code = "CashloanD001",
                               Name = "Cashloan D001"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD002",
                               Name = "Cashloan D002"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD003",
                               Name = "Cashloan D003"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD004",
                               Name = "Cashloan D004"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD005",
                               Name = "Cashloan D005"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD006",
                               Name = "Cashloan D006"
                           },
                       }
                    },
                }
            },
            new LeadPtfCategoryGroup
            {
                ProductLine = ProductLineEnum.DSA,
                Categories = new List<LeadPtfCategory>
                {
                    new LeadPtfCategory
                    {
                        Code = "CreditContract",
                        Name = "VAY THEO HỢP ĐỒNG TÍN DỤNG",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "CFA",
                                Name = "Cashloan CF A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "CFB",
                                Name = "Cashloan CF B"
                            },
                            new LeadPtfProduct
                            {
                                Code = "CFC",
                                Name = "Cashloan CF C"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "ElectricityBill",
                        Name = "VAY THEO HÓA ĐƠN ĐIỆN",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "EBA",
                                Name = "Cashloan EB A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "EBB",
                                Name = "Cashloan EB B"
                            },
                            new LeadPtfProduct
                            {
                                Code = "EBC",
                                Name = "Cashloan EB C"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "InsuranceContract",
                        Name = "VAY THEO HĐ BẢO HIỂM NHÂN THỌ",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "LIA",
                                Name = "Cashloan LI A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "LIB",
                                Name = "Cashloan LI B"
                            },
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "HealthInsuranceCard",
                        Name = "VAY THEO THẺ BHYT",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "BHYT",
                                Name = "Cashloan BHYT FAST"
                            },
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "Salary",
                        Name = "VAY THEO LƯƠNG",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "SL37",
                                Name = "Cashloan SL 37"
                            },
                            new LeadPtfProduct
                            {
                                Code = "SL42",
                                Name = "Cashloan SL 42"
                            },
                            new LeadPtfProduct
                            {
                                Code = "SLCS40",
                                Name = "Cashloan SLCS 40"
                            },
                            new LeadPtfProduct
                            {
                                Code = "SLCS45",
                                Name = "Cashloan SLCS 45"
                            },
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "PayingAccount",
                        Name = "VAY THEO TÀI KHOẢN THANH TOÁN",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "BA",
                                Name = "Cashloan BA"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "StateOfficial",
                        Name = "VAY THEO VIÊN CHỨC NHÀ NƯỚC",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "CS",
                                Name = "Cashloan CS"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "Installment",
                        Name = "VAY THEO HĐ TRẢ GÓP ĐIỆN MÁY",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "CDA",
                                Name = "Cashloan CD A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "CDB",
                                Name = "Cashloan CD B"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "CreditCard",
                        Name = "VAY THEO THẺ TÍN DỤNG",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "CC",
                                Name = "Cashloan CC"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                        Code = "MotorcycleRegistration",
                        Name = "VAY THEO ĐĂNG KÝ XE MÁY",
                        Products = new List<LeadPtfProduct>
                        {
                            new LeadPtfProduct
                            {
                                Code = "BIKEA",
                                Name = "Cashloan BIKE A"
                            },
                            new LeadPtfProduct
                            {
                                Code = "BIKEB",
                                Name = "Cashloan BIKE B"
                            }
                        }
                    },
                    new LeadPtfCategory
                    {
                       Code = "SIM",
                       Name = "VAY THEO SIM CHÍNH CHỦ",
                       Products = new List<LeadPtfProduct>
                       {
                           new LeadPtfProduct
                           {
                               Code = "Cashloan SCO 39",
                               Name = "Cashloan SCO 39"
                           },
                           new LeadPtfProduct
                           {
                               Code = "Cashloan SCO 45",
                               Name = "Cashloan SCO 45"
                           },
                       }
                    },
                    new LeadPtfCategory
                    {
                       Code = "NONSCORE",
                       Name = "NON - SCORE",
                       Products = new List<LeadPtfProduct>
                       {
                           new LeadPtfProduct
                           {
                               Code = "CashloanD001",
                               Name = "Cashloan D001"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD002",
                               Name = "Cashloan D002"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD003",
                               Name = "Cashloan D003"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD004",
                               Name = "Cashloan D004"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD005",
                               Name = "Cashloan D005"
                           },
                           new LeadPtfProduct
                           {
                               Code = "CashloanD006",
                               Name = "Cashloan D006"
                           },
                       }
                    },
                }
            }
        };

        public static IReadOnlyDictionary<string, string> CrmReturmDocumentMapping = new Dictionary<string, string>
        {
            { "CMND/CCCD/PASSPORT", "CMND/CCCD" },
            { "SỔ HỘ KHẨU", "Sổ hộ khẩu" },
            { "XÁC NHẬN CỦA KHÁCH HÀNG", string.Empty },
            { "HƯỚNG DẪN BẢN ĐỒ", string.Empty },
            { "PHIẾU THÔNG TIN KHÁCH HÀNG", string.Empty },
            { "HỒ SƠ KHÁC", "Hồ sơ khác" },
        };

        public static IReadOnlyList<ChecklistModel> Documents = new ChecklistModel[]
        {
            GenerateCheckList(ProductLineEnum.TSA, isFamilyBookMandatory: false),
            GenerateCheckList(ProductLineEnum.DSA, isFamilyBookMandatory: true),
        };

        private static ChecklistModel GenerateCheckList(string productLine, bool isFamilyBookMandatory)
        {
            return new ChecklistModel
            {
                GreenType = GreenType.GreenP,
                ProductLine = productLine,
                CategoryId = $"{LeadSourceType.PTF}",
                Checklist = new List<GroupDocument>
                {
                    new GroupDocument
                    {
                        GroupId = LeadPtfCheckListGroupId.IdentityCard,
                        GroupName = "CMND/CCCD",
                        GroupCode = "ho_so_cmnd",
                        Mandatory = true,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                Id = 1,
                                DocumentCode = "giay_to_tuy_than",
                                DocumentName = "CMND"
                            },
                            new DocumentUpload
                            {
                                Id = 2,
                                DocumentCode = "giay_to_tuy_than",
                                DocumentName = "CCCD"
                            }
                        }
                    },
                    new GroupDocument
                    {
                        GroupId = LeadPtfCheckListGroupId.FamilyBook,
                        GroupName = "Chứng minh nơi ở",
                        GroupCode = "hs_chungminh_noi_o",
                        Mandatory = true,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                Id = 1,
                                DocumentCode = "so_ho_khau",
                                DocumentName = "Sổ hộ khẩu"
                            },
                            new DocumentUpload
                            {
                                Id = 2,
                                DocumentCode = "so_tam_tru",
                                DocumentName = "Số tạm trú/Thẻ tạm trú/KT3/Hóa đơn tiện ích"
                            }
                        }
                    },
                    new GroupDocument
                    {
                        GroupId = LeadPtfCheckListGroupId.Income,
                        GroupName = "Chứng minh thu nhập",
                        GroupCode = "hs_cm_thu_nhap",
                        Mandatory = false,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                Id = 1,
                                DocumentCode = "sao_ke_tk_ngan_hang",
                                DocumentName = "Sao kê tài khoản ngân hàng"
                            },
                            new DocumentUpload
                            {
                                Id = 2,
                                DocumentCode = "bang_luong",
                                DocumentName = "Bảng lương/Phiếu lương/Giấy xác nhận lương"
                            },
                            new DocumentUpload
                            {
                                Id = 3,
                                DocumentCode = "ho_so_chung_minh_thu_nhap_khac",
                                DocumentName = "Khác"
                            }
                        }
                    },
                    new GroupDocument
                    {
                        GroupId = LeadPtfCheckListGroupId.Opcupation,
                        GroupName = "Chứng minh việc làm",
                        GroupCode = "hs_chung_minh_viec_lam",
                        Mandatory = false,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                Id = 1,
                                DocumentCode = "hop_dong_lao_dong",
                                DocumentName = "Hợp đồng lao động/Giấy xác nhận công tác"
                            },
                            new DocumentUpload
                            {
                                Id = 2,
                                DocumentCode = "tho_bao_hiem_y_te",
                                DocumentName = "Thẻ Bảo hiểm y tế"
                            },
                            new DocumentUpload
                            {
                                Id = 3,
                                DocumentCode = "ho_so_chung_minh_viec_lam_khac",
                                DocumentName = "Khác"
                            }
                        }
                    },
                    new GroupDocument
                    {
                        GroupId = LeadPtfCheckListGroupId.CustomerPicture,
                        GroupName = "Ảnh Khách hàng",
                        GroupCode = "hs_khoan_vay",
                        Mandatory = true,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                Id = 1,
                                DocumentCode = "do_de_nghi_vay_von",
                                DocumentName = "Đơn đề nghị vay vốn kiêm đăng ký sử dụng dịch vụ"
                            },
                            new DocumentUpload
                            {
                                Id = 2,
                                DocumentCode = "anh_chup_khach_hang",
                                DocumentName = "Ảnh Khách hàng"
                            },
                            new DocumentUpload
                            {
                                Id = 3,
                                DocumentCode = "ho_so_khoan_vay_khac",
                                DocumentName = "Hợp đồng tín dụng của công ty tài chính khác"
                            }
                        }
                    },
                    new GroupDocument
                    {
                        GroupId = LeadPtfCheckListGroupId.Other,
                        GroupName = "Hồ sơ khác",
                        GroupCode = "ho_so_khac",
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                Id = 4,
                                DocumentCode = "ho_so_khac",
                                DocumentName = "Khác"
                            }
                        }
                    }
                }
            };
        }
    }
}
