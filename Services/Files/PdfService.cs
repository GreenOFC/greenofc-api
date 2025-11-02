using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.File;
using _24hplusdotnetcore.Models;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using System;
using System.IO;
using System.Linq;

namespace _24hplusdotnetcore.Services.Files
{
    public interface IPdfService
    {
        byte[] GenerateCashLoan(Customer customer);
        byte[] GenerateInfomationCollectionForm(CustomerInfoPdfDto customer);
    }
    public class PdfService : IPdfService, IScopedLifetime
    {
        public byte[] GenerateCashLoan(Customer customer)
        {
            const string template = @"Templates/Template_DN_20201110_2.pdf";
            using (MemoryStream stream = new MemoryStream())
            {
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(template), new PdfWriter(stream));
                PdfFont font = PdfFontFactory.CreateFont(@"Fonts/ARIALUNI.TTF", PdfEncodings.IDENTITY_H, true);

                PdfPage page = pdfDoc.GetPage(1);
                var canvas = new PdfCanvas(page);

                canvas.SetFillColor(ColorConstants.WHITE);
                //0.1
                {
                    //Số hợp đồng
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(360, 991)
                        .ShowText("")
                        .EndText();

                    // Ngày
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(520, 991).ShowText("")
                        .MoveText(35, 0).ShowText("")
                        .MoveText(45, 0).ShowText("")
                        .EndText();
                }
                //0.2
                {
                    // Mã nhân viên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(365, 967)
                        .ShowText("24H-TE0001")
                        .EndText();

                    // Tên nhân viên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(557, 967)
                        .ShowText(customer.SaleInfo.Name.ToUpper())
                        .EndText();
                }
                //0.3
                {
                    // Mã DC
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(335, 948)
                        .ShowText("")
                        .EndText();

                    // Tên DC
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(525, 948)
                        .ShowText("")
                        .EndText();
                }

                canvas.SetFillColor(ColorConstants.BLACK);
                //1.1
                {
                    //Họ tên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(100, 829)
                        .ShowText(customer.Personal.Name.ToUpper())
                        .EndText();

                    //Tên gọi khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(435, 829)
                        .ShowText("")
                        .EndText();
                }
                //1.2
                {
                    if (customer.Personal.Gender == "Nam")
                    {
                        // Nam
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(106, 798)
                            .ShowText("x")
                            .EndText();
                    }
                    else
                    {
                        // Nữ
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(154, 798)
                            .ShowText("x")
                            .EndText();
                    }

                    var dob = customer.Personal.DateOfBirth.Split("/");
                    // Ngày sinh
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(426, 801).ShowText(dob[0])
                        .MoveText(30, 0).ShowText(dob[1])
                        .MoveText(30, 0).ShowText(dob[2])
                        .EndText();
                }
                //1.3
                {
                    //CMND
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(137, 775)
                        .ShowText(customer.Personal.IdCard)
                        .EndText();

                    var issueDate = customer.Personal.IdCardDate.Split("/");
                    //Ngày cấp, nơi cấp
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(426, 775).ShowText(issueDate[0])
                        .MoveText(30, 0).ShowText(issueDate[1])
                        .MoveText(30, 0).ShowText(issueDate[2])
                        .MoveText(103, 0).ShowText(customer.Personal.IdCardProvince.ToUpper())
                        .EndText();
                }
                //1.4
                {
                    //CMND cũ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(155, 750)
                        .ShowText(customer.Personal.OldIdCard)
                        .EndText();
                }
                //1.5
                {
                    switch (customer.Personal.MaritalStatus)
                    {
                        case "Độc thân":
                            // Độc thân
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(165, 719)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "Đã kết hôn":
                            // Lập gia đình
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(260, 719)
                                .ShowText("x")
                                .EndText();

                            break;
                        case "Góa":
                            // Góa bụa
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(354, 719)
                                .ShowText("x")
                                .EndText();

                            break;
                        case "Ly hôn":
                            // Li dị
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(448, 719)
                                .ShowText("x")
                                .EndText();

                            break;
                        case "Khác":
                            // Khác
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(542, 719)
                                .ShowText("x")
                                .EndText();
                            break;
                        default:
                            break;
                    }
                    // khác text
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(585, 721)
                        .ShowText("")
                        .EndText();

                }
                //1.6
                {
                    switch (customer.Personal.EducationLevel)
                    {
                        case "Tiểu học":
                            // Tiểu học
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(165, 693)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "THCS":
                            // THCS
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(260, 693)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "Phổ thông":
                            //THPT
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(354, 693)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "Trung cấp":
                            // Trung cấp
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(448, 693)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "Cao đẳng":
                            // Cao đẳng
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(165, 668)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "Đại học":
                            //đại học
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(260, 668)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "Sau đại học":
                            //Sau đại học
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(354, 668)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "Khác":
                            // Khác
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(448, 668)
                                .ShowText("x")
                                .EndText();
                            // Khác
                            canvas.BeginText()
                                .SetFontAndSize(font, 13)
                                .MoveText(497, 670)
                                .ShowText("")
                                .EndText();
                            break;
                        default:
                            break;
                    }



                }
                //1.8
                {
                    //DTDD
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(95, 642)
                        .ShowText(customer.Personal.Phone)
                        .EndText();

                    //Email
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(297, 642)
                        .ShowText(customer.Personal.Email)
                        .EndText();

                    // số người phụ thuộc
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(653, 642)
                        .ShowText(customer.Personal.NoOfDependent)
                        .EndText();
                }

                //2.1
                {
                    //Địa chỉ nơi ở hiện tại
                    canvas.BeginText()
                        .SetFontAndSize(font, 10)
                        .MoveText(165, 572)
                        .ShowText(customer.TemporaryAddress.GetFullAddress().ToUpper())
                        .EndText();
                }
                //2.2
                {
                    //Thời gian sinh sống
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(160, 548)
                        .ShowText(customer.TemporaryAddress.DurationYear.ToString())
                        .MoveText(56, 0)
                        .ShowText(customer.TemporaryAddress.DurationMonth.ToString())
                        .EndText();

                    //Số người sinh sống
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(400, 548)
                        .ShowText("")
                        .EndText();

                    //Điện thoại bàn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(562, 548)
                        .ShowText(customer.TemporaryAddress.FixedPhone)
                        .EndText();
                }
                //2.3
                {
                    string propertyStatus = customer.TemporaryAddress.GetMAFCPropertyStatus();
                    switch (propertyStatus)
                    {
                        case "O":
                            // Sở hữu
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(220, 521)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "R":
                            // Thuê
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(298, 521)
                                .ShowText("x")
                                .EndText();

                            // Tên chủ nhà trọ
                            canvas.BeginText()
                                .SetFontAndSize(font, 13)
                                .MoveText(145, 474)
                                .ShowText(customer.TemporaryAddress.LandLordName.ToUpper())
                                .EndText();

                            // Thuê nguyên căn
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(347, 472)
                                .ShowText("")
                                .EndText();

                            // Thuê phòng trọ
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(461, 472)
                                .ShowText("x")
                                .EndText();

                            // Phòng số
                            canvas.BeginText()
                                .SetFontAndSize(font, 13)
                                .MoveText(612, 474)
                                .ShowText(customer.TemporaryAddress.RoomNo)
                                .EndText();
                            break;
                        case "F":
                            // Nhà người thân
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(367, 521)
                                .ShowText("x")
                                .EndText();
                            break;
                        default:
                            // Khác
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(482, 521)
                                .ShowText("x")
                                .EndText();

                            // Khác
                            canvas.BeginText()
                                .SetFontAndSize(font, 13)
                                .MoveText(527, 522)
                                .ShowText("")
                                .EndText();
                            break;
                    }
                }
                //2.5
                {
                    // Mô tả đường đi
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(145, 448)
                        .ShowText(customer.TemporaryAddress.LandMark.ToUpper())
                        .EndText();
                }
                //2.6
                {
                    if (customer.IsTheSameResidentAddress)
                    {
                        // Giống với địa chỉ nơi ở hiện tại
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(143, 423)
                            .ShowText("x")
                            .EndText();
                    }
                    else
                    {
                        // Khác với địa chỉ nơi ở hiện tại
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(333, 423)
                            .ShowText("x")
                            .EndText();
                        //Địa chỉ hộ khẩu
                        canvas.BeginText()
                            .SetFontAndSize(font, 10)
                            .MoveText(145, 400)
                            .ShowText(customer.ResidentAddress.GetFullAddress().ToUpper())
                            .EndText();
                    }
                }
                //2.8
                {
                    //Số hộ khẩu
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(135, 375)
                        .ShowText(customer.FamilyBookNo)
                        .EndText();

                    //Số điện thoại bàn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(417, 375)
                        .ShowText(customer.ResidentAddress.FixedPhone)
                        .EndText();
                }
                //2.9
                {
                    //Mô tả đường đi
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(140, 350)
                        .ShowText("")
                        .EndText();
                }

                //3.1
                {
                    string purpose = customer.Loan.GetMAFCPurpose();
                    switch (purpose)
                    {
                        case "A":
                            // Mua hàng
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(164, 272)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "M":
                            // Sửa nhà
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(258, 272)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "H":
                            // Chi phí y tế
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(346, 272)
                                .ShowText("x")
                                .EndText();
                            break;
                        case "P":
                            // Khác
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(439, 272)
                                .ShowText("x")
                                .EndText();
                            // Khác
                            canvas.BeginText()
                                .SetFontAndSize(font, 13)
                                .MoveText(485, 275)
                                .ShowText("")
                                .EndText();
                            break;
                        default:
                            break;
                    }
                }
                //3.2
                {
                    // Ngày đề nghị thanh toán
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(230, 249)
                        .ShowText(customer.Working.DueDay)
                        .EndText();
                }
                //3.3
                {
                    switch (customer.Loan.Category)
                    {
                        case "Employee Cash Loan":
                            // Employee Cash Loan
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(49, 200)
                                .ShowText("x")
                                .EndText();
                            // Employee Cash Loan
                            canvas.BeginText()
                                .SetFontAndSize(font, 8)
                                .MoveText(175, 202)
                                .ShowText(customer.Loan.Product.ToUpper())
                                .EndText();
                            break;
                        case "Self-Employee":
                            // Self-Employee
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(281, 200)
                                .ShowText("x")
                                .EndText();

                            //  Self-Employee
                            canvas.BeginText()
                                .SetFontAndSize(font, 8)
                                .MoveText(373, 202)
                                .ShowText(customer.Loan.Product.ToUpper())
                                .EndText();

                            break;
                        case "NEW FAST LOAN":
                            // Fast Loan
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(495, 200)
                                .ShowText("x")
                                .EndText();

                            //  Fast Loan
                            canvas.BeginText()
                                .SetFontAndSize(font, 8)
                                .MoveText(565, 202)
                                .ShowText(customer.Loan.Product.ToUpper())
                                .EndText();
                            break;
                        case "SURROGATE":
                            if (customer.Loan.Product.IndexOf("UCCC") > -1)
                            {
                                // UCCC
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(49, 175)
                                    .ShowText("x")
                                    .EndText();

                                // UCCC
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(100, 177)
                                    .ShowText(customer.Loan.Product.ToUpper())
                                    .EndText();
                            }
                            else if (customer.Loan.Product.IndexOf("EVN") > -1)
                            {
                                // EVN
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(281, 175)
                                    .ShowText("x")
                                    .EndText();

                                // EVN
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(325, 177)
                                    .ShowText(customer.Loan.Product.ToUpper())
                                    .EndText();
                            }
                            else if (customer.Loan.Product.IndexOf("UBS") > -1)
                            {
                                // UBS
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(495, 175)
                                    .ShowText("x")
                                    .EndText();

                                // UBS
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(585, 177)
                                    .ShowText(customer.Loan.Product.ToUpper())
                                    .EndText();
                            }
                            else if (customer.Loan.Product.IndexOf("WATER") > -1)
                            {
                                // Water CL
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(49, 151)
                                    .ShowText("x")
                                    .EndText();

                                // Water CL
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(115, 153)
                                    .ShowText(customer.Loan.Product.ToUpper())
                                    .EndText();

                            }
                            else if (customer.Loan.Product.IndexOf("POST-PAID") > -1)
                            {
                                // Post-Paid
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(281, 151)
                                    .ShowText("x")
                                    .EndText();

                                // Post-Paid
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(362, 153)
                                    .ShowText(customer.Loan.Product.ToUpper())
                                    .EndText();

                            }
                            else if (customer.Loan.Product.StartsWith("CC"))
                            {
                                // CC
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(495, 151)
                                    .ShowText("x")
                                    .EndText();

                                // CC
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(600, 153)
                                    .ShowText(customer.Loan.Product.ToUpper())
                                    .EndText();
                            }
                            else if (customer.Loan.Product.IndexOf("LIFE") > -1)
                            {
                                // Life-Insurance
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(49, 127)
                                    .ShowText("x")
                                    .EndText();

                                // Life-Insurance
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(137, 129)
                                    .ShowText(customer.Loan.Product.ToUpper())
                                    .EndText();

                                // Định kỳ
                                canvas.BeginText()
                                    .SetFontAndSize(font, 13)
                                    .MoveText(412, 129)
                                    .ShowText("")
                                    .EndText();

                                // Số tiền trả định kỳ
                                canvas.BeginText()
                                    .SetFontAndSize(font, 13)
                                    .MoveText(595, 129)
                                    .ShowText("")
                                    .EndText();
                            }
                            else if (customer.Loan.Product.IndexOf("BAS") > -1)
                            {
                                // BAS
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(49, 104)
                                    .ShowText("x")
                                    .EndText();

                                // BAS
                                canvas.BeginText()
                                    .SetFontAndSize(font, 8)
                                    .MoveText(145, 105)
                                    .ShowText("")
                                    .EndText();
                            }
                            else
                            {
                                // Khác
                                canvas.BeginText()
                                    .SetFontAndSize(font, 17)
                                    .MoveText(281, 103)
                                    .ShowText("x")
                                    .EndText();

                                // Khác
                                canvas.BeginText()
                                    .SetFontAndSize(font, 13)
                                    .MoveText(325, 105)
                                    .ShowText("")
                                    .EndText();
                            }
                            break;
                        default:
                            break;
                    }

                }

                page = pdfDoc.GetPage(2);
                canvas = new PdfCanvas(page);

                //3.8
                {
                    // Khoản vay tiêu dùng
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(180, 982)
                        .ShowText(customer.Loan.Amount + " (vnđ)")
                        .EndText();

                    // Bằng chữ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(350, 982)
                        .ShowText("")
                        .EndText();

                    // Thời hạn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(640, 982)
                        .ShowText(customer.Loan.Term)
                        .EndText();
                }
                //3.9
                {
                    if (!customer.Loan.BuyInsurance)
                    {
                        // Không mua bảo hiểm
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(90, 937)
                            .ShowText("Không mua bảo hiểm")
                            .EndText();
                    }
                }
                //3.10
                {
                    string loanAmount = customer.Loan.Amount.Replace(",", string.Empty);
                    double.TryParse(loanAmount, out double amout);
                    amout = amout + amout * 0.55;
                    // Tổng tiền bao gồm bảo hiểm
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(365, 910)
                        .ShowText(amout.ToString() + "(vnđ)")
                        .EndText();
                }
                //4.1
                {
                    int consti = customer.Working.GetMAFCConsti();
                    if (consti == 5)
                    {
                        // Từ lương
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(168, 845)
                            .ShowText("x")
                            .EndText();
                    }
                    else
                    {
                        // Từ kinh doanh
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(288, 845)
                            .ShowText("x")
                            .EndText();
                    }
                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(451, 845)
                        .ShowText("")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(495, 847)
                        .ShowText("")
                        .EndText();
                }
                //4.2
                {
                    // Công ty
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(120, 819)
                        .ShowText(customer.Working.CompanyName.ToUpper())
                        .EndText();
                }
                //4.3
                {
                    // Địa chỉ
                    canvas.BeginText()
                        .SetFontAndSize(font, 10)
                        .MoveText(100, 792)
                        .ShowText(customer.Working.CompanyAddress.GetFullAddress().ToUpper())
                        .EndText();
                }
                //4.5
                {
                    if (customer.Working.CompanyAddress.Type == "HEADOFF")
                    {
                        // Trụ sở chính
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(49, 763)
                            .ShowText("x")
                            .EndText();
                    }
                    else
                    {
                        // Chi nhánh
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(158, 763)
                            .ShowText("x")
                            .EndText();
                    }
                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(258, 763)
                        .ShowText("")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(300, 765)
                        .ShowText("")
                        .EndText();

                    // MST
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(435, 765)
                        .ShowText(customer.Working.CompanyTaxCode)
                        .EndText();

                    // Điện thoại bàn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(610, 765)
                        .ShowText(customer.Working.CompanyPhone + string.Empty)
                        .EndText();
                }
                //4.6
                {
                    // Thời gian làm việc - năm
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(152, 735)
                        .ShowText(customer.Working.CompanyAddress.DurationYear + string.Empty)
                        .EndText();

                    // Thời gian làm việc - tháng
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(210, 735)
                        .ShowText(customer.Working.CompanyAddress.DurationMonth + string.Empty)
                        .EndText();

                    // Vị trí
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(355, 735)
                        .ShowText(customer.Working.Position.ToUpper())
                        .EndText();
                }
                //5.1
                {
                    // Thu nhập chính
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(140, 675)
                        .ShowText(customer.Working.Income)
                        .EndText();

                    // Thu nhập khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(465, 675)
                        .ShowText("")
                        .EndText();
                }
                //5.2
                {
                    if (customer.Working.Constitution == "Từ lương")
                    {
                        if (customer.Working.Priority == "Tài khoản ngân hàng")
                        {
                            // TK ngân hàng
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(174, 612)
                                .ShowText("x")
                                .EndText();
                        }
                        else if (customer.Working.Priority == "Tiền mặt")
                        {
                            // Tiền mặt
                            canvas.BeginText()
                                .SetFontAndSize(font, 17)
                                .MoveText(275, 612)
                                .ShowText("x")
                                .EndText();
                        }
                        // Ngày nhận lương
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(500, 613)
                            .ShowText("05")
                            .EndText();
                    }
                }
                //6.1
                {
                    if (customer.Personal.MaritalStatus == "Đã kết hôn")
                    {
                        // Họ tên
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(95, 546)
                            .ShowText(customer.Spouse.Name.ToUpper())
                            .EndText();

                        // CMND
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(330, 546)
                            .ShowText(customer.Spouse.IdCard)
                            .EndText();

                        // DTDD
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(540, 546)
                            .ShowText(customer.Spouse.Phone)
                            .EndText();
                    }
                }
                //6.2
                {
                    // Tên công ty
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(115, 516)
                        .ShowText("")
                        .EndText();

                    // Địa chỉ công ty
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(365, 516)
                        .ShowText("")
                        .EndText();
                }
                //7.1
                {
                    Referee referee1 = customer.Referees.ToList()[0];
                    // Họ tên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(105, 455)
                        .ShowText(referee1.Name.ToUpper())
                        .EndText();

                    // DT liên hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(345, 455)
                        .ShowText(referee1.Phone)
                        .EndText();

                    // Mối quan hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(620, 455)
                        .ShowText(referee1.Relationship.ToUpper())
                        .EndText();
                }
                //7.2
                {
                    Referee referee2 = customer.Referees.ToList()[1];
                    // Họ tên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(105, 425)
                        .ShowText(referee2.Name.ToUpper())
                        .EndText();

                    // DT liên hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(345, 425)
                        .ShowText(referee2.Phone)
                        .EndText();

                    // Mối quan hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(630, 425)
                        .ShowText(referee2.Relationship.ToUpper())
                        .EndText();
                }
                //8.1
                {
                    // Tên TCTD
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(150, 360)
                        .ShowText("")
                        .EndText();

                    // Ngày vay
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(410, 360)
                        .ShowText("")
                        .EndText();

                    // Ngày đén hạn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(605, 360)
                        .ShowText("")
                        .EndText();
                }
                //8.2
                {
                    // Dư nợ hiện tại
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(120, 332)
                        .ShowText("")
                        .EndText();

                    // Số tiền trả hàng tháng
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(475, 332)
                        .ShowText("")
                        .EndText();
                }
                //9.1
                {
                    if (customer.BankInfo.IsBankAccount)
                    {
                        // Ngân hàng
                        canvas.BeginText()
                            .SetFontAndSize(font, 8)
                            .MoveText(115, 260)
                            .ShowText(customer.BankInfo.Name.ToUpper())
                            .EndText();

                        // Chi nhánh
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(343, 260)
                            .ShowText(customer.BankInfo.Branch.ToUpper())
                            .EndText();

                        // Số tài khoản
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(515, 260)
                            .ShowText(customer.BankInfo.AccountNo)
                            .EndText();
                    }
                }
                //10.1
                {
                    // Người thân
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(211, 183)
                        .ShowText("")
                        .EndText();

                    // Vợ chồng
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(324, 183)
                        .ShowText("")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(452, 183)
                        .ShowText("")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(495, 185)
                        .ShowText("")
                        .EndText();
                }
                //10.2
                {
                    // Ghi chú
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(60, 143)
                        .ShowText("")
                        .EndText();
                }

                pdfDoc.Close();

                return stream.ToArray();
            }

        }

        public byte[] GenerateInfomationCollectionForm(CustomerInfoPdfDto customer)
        {
            const string template = @"Templates/Template_DN_20201110_2.pdf";
            using (MemoryStream stream = new MemoryStream())
            {
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(template), new PdfWriter(stream));
                PdfFont font = PdfFontFactory.CreateFont(@"Fonts/ARIALUNI.TTF", PdfEncodings.IDENTITY_H, true);

                PdfPage page = pdfDoc.GetPage(1);
                var canvas = new PdfCanvas(page);

                canvas.SetFillColor(ColorConstants.WHITE);

                string propertyStatus = customer.TemporaryAddress?.GetMAFCPropertyStatus();
                int consti = customer.Working.GetMAFCConsti();
                string loanAmount = customer.Loan?.Amount?.Replace(",", string.Empty);
                double.TryParse(loanAmount, out double totalLoan);
                if (customer.Loan.BuyInsurance)
                {
                    totalLoan += totalLoan * 0.077;
                }
                //0.2
                {
                    DateTime today = DateTime.Now;
                    // Ngay thang nam
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(453, 982).ShowText($"{today.Day}")
                        .MoveText(35, 0).ShowText($"{today.Month}")
                        .MoveText(45, 0).ShowText($"{today.Year}")
                        .EndText();

                    // Mã nhân viên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(310, 956)
                        .ShowText($"{customer.SaleInfo?.MAFCCode?.ToUpper()}")
                        .EndText();

                    // Tên nhân viên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(495, 956)
                        .ShowText($"{customer.SaleInfo?.MAFCName?.ToUpper()}")
                        .EndText();
                }

                canvas.SetFillColor(ColorConstants.BLACK);

                //1.1
                {
                    //Họ tên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(100, 785)
                        .ShowText($"{customer.Personal?.Name}")
                        .EndText();
                }
                //1.2
                {
                    // Nam
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(106, 750)
                        .ShowTextIf(customer.Personal?.Gender == "Nam", "x")
                        .EndText();

                    // Nữ
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(154, 750)
                        .ShowTextIf(customer.Personal?.Gender != "Nam", "x")
                        .EndText();

                    var dateOfBirth = customer.Personal?.GetDateOfBirth();
                    // Ngày sinh
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(520, 752).ShowTextIf(dateOfBirth.HasValue == true, $"{dateOfBirth.Value.Day}")
                        .MoveText(30, 0).ShowTextIf(dateOfBirth.HasValue == true, $"{dateOfBirth.Value.Month}")
                        .MoveText(30, 0).ShowTextIf(dateOfBirth.HasValue == true, $"{dateOfBirth.Value.Year}")
                        .EndText();
                }
                //1.3
                {
                    //CMND
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(137, 722)
                        .ShowText($"{customer.Personal?.IdCard}")
                        .EndText();

                    var issueDate = customer.Personal?.GetIdCardDate();
                    //Ngày cấp, nơi cấp
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(362, 722).ShowTextIf(issueDate.HasValue == true, $"{issueDate.Value.Day}")
                        .MoveText(25, 0).ShowTextIf(issueDate.HasValue == true, $"{issueDate.Value.Month}")
                        .MoveText(23, 0).ShowTextIf(issueDate.HasValue == true, $"{issueDate.Value.Year}")
                        .EndText();

                    //Nơi cấp
                    canvas.BeginText()
                        .SetFontAndSize(font, 9)
                        .MoveText(505, 722).ShowText($"{customer.Personal?.IdCardProvince}")
                        .EndText();
                }
                //1.4
                {
                    //CMND cũ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(155, 692)
                        .ShowText($"{customer.Personal?.OldIdCard}")
                        .EndText();
                }
                //1.5
                {
                    // Độc thân
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(166, 660)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Độc thân", "x")
                        .EndText();

                    // Lập gia đình
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(260, 661)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Đã kết hôn", "x")
                        .EndText();

                    // Góa bụa
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(354, 661)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Góa", "x")
                        .EndText();

                    // Li dị
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(448, 661)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Ly hôn", "x")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(542, 661)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Khác" || customer.Personal?.MaritalStatus == "Ly thân", "x")
                        .EndText();

                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(590, 661)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Khác" || customer.Personal?.MaritalStatus == "Ly thân", customer.Personal?.OtherMaritalStatus ?? "")
                        .EndText();

                }
                //1.6
                {
                    // Tiểu học
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(165, 630)
                        .ShowTextIf(customer.Personal?.EducationLevel == "Tiểu học", "x")
                        .EndText();

                    // THCS
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(260, 630)
                        .ShowTextIf(customer.Personal?.EducationLevel == "THCS", "x")
                        .EndText();

                    //THPT
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(354, 630)
                        .ShowTextIf(customer.Personal?.EducationLevel == "Phổ thông", "x")
                        .EndText();

                    // Trung cấp
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(448, 630)
                        .ShowTextIf(customer.Personal?.EducationLevel == "Trung cấp", "x")
                        .EndText();
                }
                //1.7
                {
                    // Cao đẳng
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(166, 601)
                        .ShowTextIf(customer.Personal?.EducationLevel == "Cao đẳng", "x")
                        .EndText();

                    //đại học
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(260, 601)
                        .ShowTextIf(customer.Personal?.EducationLevel == "Đại học", "x")
                        .EndText();

                    //Sau đại học
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(354, 601)
                        .ShowTextIf(customer.Personal?.EducationLevel == "Sau đại học", "x")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(448, 601)
                        .ShowTextIf(customer.Personal?.EducationLevel == "Khác", "x")
                        .EndText();
                }
                //1.8
                {
                    //DTDD
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(95, 569)
                        .ShowText($"{customer.Personal?.Phone}")
                        .EndText();

                    //Email
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(315, 569)
                        .ShowText($"{customer.Personal?.Email}")
                        .EndText();

                    // số người phụ thuộc
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(665, 569)
                        .ShowText($"{customer.Personal?.NoOfDependent}")
                        .EndText();
                }
                //2.1
                {
                    //Địa chỉ nơi ở hiện tại
                    var tempAddress = SplitAddress(customer.TemporaryAddress?.GetFullAddress()?.ToUpper());
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(165, 482)
                        .ShowText(tempAddress[0])
                        .EndText();
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 452)
                        .ShowText(tempAddress[1] + tempAddress[2])
                        .EndText();
                }
                //2.2
                {
                    //Thời gian sinh sống
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(160, 414)
                        .ShowText($"{customer.TemporaryAddress?.DurationYear}")
                        .MoveText(56, 0)
                        .ShowText($"{customer.TemporaryAddress?.DurationMonth}")
                        .EndText();

                    //Điện thoại bàn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(562, 417)
                        .ShowText($"{customer.TemporaryAddress?.FixedPhone}")
                        .EndText();
                }
                //2.3
                {
                    // Sở hữu
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(220, 379)
                        .ShowTextIf(propertyStatus == "O", "x")
                        .EndText();

                    // Thuê
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(298, 379)
                        .ShowTextIf(propertyStatus == "R", "x")
                        .EndText();

                    // Nhà người thân
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(367, 379)
                        .ShowTextIf(propertyStatus == "F", "x")
                        .EndText();

                    // Khác
                    // canvas.BeginText()
                    //     .SetFontAndSize(font, 17)
                    //     .MoveText(482, 379)
                    //     .ShowText("x")
                    //     .EndText();
                }
                // 2.4
                {
                    if (propertyStatus == "R")
                    {
                        // Tên chủ nhà trọ
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(145, 330)
                            .ShowTextIf(propertyStatus == "R", customer.TemporaryAddress?.LandLordName?.ToUpper())
                            .EndText();

                        // Thuê phòng trọ
                        canvas.BeginText()
                            .SetFontAndSize(font, 17)
                            .MoveText(461, 330)
                            .ShowTextIf(propertyStatus == "R", "x")
                            .EndText();

                        // Phòng số
                        canvas.BeginText()
                            .SetFontAndSize(font, 13)
                            .MoveText(612, 330)
                            .ShowTextIf(propertyStatus == "R", customer.TemporaryAddress?.RoomNo?.ToUpper())
                            .EndText();
                    }
                }
                //2.5
                {
                    // Mô tả đường đi
                    var landMark = SplitAddress(customer.TemporaryAddress?.LandMark?.ToUpper());
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(145, 298)
                        .ShowText(landMark[0])
                        .EndText();
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 269)
                        .ShowText(landMark[1] + landMark[2])
                        .EndText();
                }
                //2.6
                {
                    // Giống với địa chỉ nơi ở hiện tại
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(143, 238)
                        .ShowTextIf(customer.IsTheSameResidentAddress, "x")
                        .EndText();

                    // Khác với địa chỉ nơi ở hiện tại
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(333, 238)
                        .ShowTextIf(!customer.IsTheSameResidentAddress, "x")
                        .EndText();
                }
                //2.7
                {
                    var resAddress = SplitAddress(customer.ResidentAddress?.GetFullAddress()?.ToUpper());
                    //Địa chỉ hộ khẩu
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(145, 215)
                        .ShowTextIf(!customer.IsTheSameResidentAddress, resAddress[0])
                        .EndText();
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 186)
                        .ShowTextIf(!customer.IsTheSameResidentAddress, resAddress[1] + resAddress[2])
                        .EndText();
                }
                //2.8
                {
                    //Số hộ khẩu
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(135, 157)
                        .ShowText($"{customer.FamilyBookNo?.ToUpper()}")
                        .EndText();

                    //Số điện thoại bàn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(417, 157)
                        .ShowText($"{customer.ResidentAddress?.FixedPhone?.ToUpper()}")
                        .EndText();
                }


                page = pdfDoc.GetPage(2);
                canvas = new PdfCanvas(page);

                //3.1
                {
                    string purpose = customer.Loan?.GetMAFCPurpose();
                    // Mua hàng
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(158, 946)
                        .ShowTextIf(purpose == "A", "x")
                        .EndText();

                    // Sửa nhà
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(252, 946)
                        .ShowTextIf(purpose == "H", "x")
                        .EndText();

                    // Chi phí y tế
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(340, 946)
                        .ShowTextIf(purpose == "M", "x")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(433, 946)
                        .ShowTextIf(purpose == "P", "x")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(483, 946)
                        .ShowTextIf(purpose == "P", customer.Loan?.PurposeOther)
                        .EndText();
                }
                //3.2
                {
                    // Ngày đề nghị thanh toán
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(270, 920)
                        .ShowText(!string.IsNullOrEmpty(customer.Loan?.PaymentDate) ? customer.Loan.PaymentDate : $"{customer.Working?.DueDay}")
                        .EndText();
                }
                //3.3
                {
                    // Employee Cash Loan
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 868)
                        .ShowTextIf(customer.Loan.Category == "Employee Cash Loan", "x")
                        .EndText();

                    // Employee Cash Loan
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(170, 869)
                        .ShowTextIf(customer.Loan.Category == "Employee Cash Loan", customer.Loan.Product.ToUpper())
                        .EndText();

                    // Self-Employee
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(397, 868)
                        .ShowTextIf(customer.Loan.Category == "Self-Employee", "x")
                        .EndText();

                    //  Self-Employee
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(490, 869)
                        .ShowTextIf(customer.Loan.Category == "Self-Employee", customer.Loan.Product.ToUpper())
                        .EndText();
                }
                //3.4
                {
                    // Fast Loan
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 838)
                        .ShowTextIf(customer.Loan.Category == "NEW FAST LOAN", "x")
                        .EndText();

                    //  Fast Loan
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(115, 839)
                        .ShowTextIf(customer.Loan.Category == "NEW FAST LOAN", customer.Loan.Product.ToUpper())
                        .EndText();

                    // UCCC
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(397, 838)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("UCCC"), "x")
                        .EndText();

                    // UCCC
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(450, 839)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("UCCC"), customer.Loan.Product.ToUpper())
                        .EndText();
                }
                //3.5
                {
                    // EVN
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 808)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("EVN"), "x")
                        .EndText();

                    // EVN
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(90, 809)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("EVN"), customer.Loan.Product.ToUpper())
                        .EndText();

                    // UBS
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(397, 808)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("UBS"), "x")
                        .EndText();

                    // UBS
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(495, 809)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("UBS"), customer.Loan.Product.ToUpper())
                        .EndText();
                }
                //3.6
                {
                    // Water CL
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 778)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("WATER"), "x")
                        .EndText();

                    // Water CL
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(115, 779)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("WATER"), customer.Loan.Product.ToUpper())
                        .EndText();

                    // Post-Paid
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(397, 778)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.IndexOf("POST-PAID") > -1, "x")
                        .EndText();

                    // Post-Paid
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(480, 779)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.IndexOf("POST-PAID") > -1, customer.Loan.Product.ToUpper())
                        .EndText();
                }
                //3.7
                {
                    // CC
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 749)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("CC"), "x")
                        .EndText();

                    // CC
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(150, 750)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("CC"), customer.Loan.Product.ToUpper())
                        .EndText();

                    // BAS
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(397, 749)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("BAS"), "x")
                        .EndText();

                    // BAS
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(490, 750)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("BAS"), customer.Loan.Product.ToUpper())
                        .EndText();
                }
                //3.8
                {
                    // Life-Insurance
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 718)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("LIFE"), "x")
                        .EndText();

                    // Life-Insurance
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(137, 719)
                        .ShowTextIf(customer.Loan.Category == "SURROGATE"
                                && customer.Loan.Product.StartsWith("LIFE"), customer.Loan?.Product?.ToUpper())
                        .EndText();

                    // Định kỳ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(575, 720)
                        .ShowText("")
                        .EndText();
                }
                //3.9
                {
                    // Số tiền trả định kỳ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(145, 690)
                        .ShowText("")
                        .EndText();
                }
                //3.10
                {
                    var otherCategories = new[] { "TEACHER", "HOSPITAL", "MY FINANCE" };
                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 658)
                        .ShowTextIf(otherCategories.Contains(customer.Loan?.Category), "x")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(100, 658)
                        .ShowTextIf(otherCategories.Contains(customer.Loan?.Category), customer.Loan?.Product?.ToUpper())
                        .EndText();
                }
                //3.11
                {
                    // Khoản vay tiêu dùng
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(175, 589)
                        .ShowText(FormatMoney(customer.Loan?.Amount))
                        .EndText();

                    // Bằng chữ
                    canvas.BeginText()
                        .SetFontAndSize(font, 9)
                        .MoveText(345, 589)
                        .ShowText(GetStringMoney(loanAmount)?.ToUpper())
                        .EndText();

                    // Thời hạn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(635, 589)
                        .ShowText(customer.Loan?.Term)
                        .EndText();
                }
                //3.12
                {
                    // Không mua bảo hiểm
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(90, 533)
                        .ShowText(customer.Loan?.BuyInsurance == true ? "Có" : "Không")
                        .EndText();
                }
                //3.13
                {
                    // Tổng tiền bao gồm bảo hiểm
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(365, 504)
                        .ShowText(FormatMoney(totalLoan.ToString()))
                        .EndText();
                }
                //4.1
                {
                    // Từ lương
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(163, 412)
                        .ShowTextIf(consti == 5, "x")
                        .EndText();

                    // Từ kinh doanh
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(283, 412)
                        .ShowTextIf(consti == 8, "x")
                        .EndText();
                }
                //4.2
                {
                    // Công ty
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(120, 386)
                        .ShowText(customer.Working?.CompanyName.ToUpper())
                        .EndText();
                }
                //4.3
                {
                    // Địa chỉ
                    var comAddress = SplitAddress(customer.Working?.CompanyAddress?.GetFullAddress()?.ToUpper());
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(100, 358)
                        .ShowText(comAddress[0])
                        .EndText();
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 330)
                        .ShowText(comAddress[1] + comAddress[2])
                        .EndText();
                }
                //4.4
                {
                    // Trụ sở chính
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(43, 293)
                        .ShowTextIf(customer.Working?.CompanyAddress?.Type == "HEADOFF", "x")
                        .EndText();

                    // Chi nhánh
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(153, 293)
                        .ShowTextIf(customer.Working?.CompanyAddress?.Type == "BCHOFF", "x")
                        .EndText();

                    var taxCode = customer.Working?.CompanyTaxCode;
                    if (!string.IsNullOrEmpty(customer.Working?.CompanyTaxSubCode))
                    {
                        taxCode += "-" + customer.Working?.CompanyTaxSubCode;
                    }
                    // MST
                    canvas.BeginText()
                        .SetFontAndSize(font, 10)
                        .MoveText(428, 293)
                        .ShowText($"{taxCode}")
                        .EndText();

                    // Điện thoại bàn
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(605, 293)
                        .ShowText(customer.Working?.CompanyPhone + string.Empty)
                        .EndText();
                }
                //4.5
                {
                    // Thời gian làm việc - năm
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(150, 265)
                        .ShowText(customer.Working?.CompanyAddress?.DurationYear + string.Empty)
                        .EndText();

                    // Thời gian làm việc - tháng
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(215, 265)
                        .ShowText(customer.Working?.CompanyAddress?.DurationMonth + string.Empty)
                        .EndText();

                    // Vị trí
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(355, 265)
                        .ShowText(customer.Working?.Position?.ToUpper())
                        .EndText();
                }
                //5.1
                {
                    // Thu nhập chính
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(135, 184)
                        .ShowText(FormatMoney(customer.Working?.Income))
                        .EndText();
                }
                //5.2
                {
                    // TK ngân hàng
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(169, 120)
                        .ShowTextIf(consti == 5
                            && customer.Working?.IncomeMethod == "Tài khoản ngân hàng", "x")
                        .EndText();

                    // Tiền mặt
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(269, 120)
                        .ShowTextIf(consti == 5
                            && customer.Working?.IncomeMethod == "Tiền mặt", "x")
                        .EndText();

                    // Ngày nhận lương
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(540, 120)
                        .ShowTextIf(consti == 5, customer.Working?.DueDay)
                        .EndText();
                }

                page = pdfDoc.GetPage(3);
                canvas = new PdfCanvas(page);

                //6.1
                {
                    // Họ tên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(95, 948)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Đã kết hôn", customer.Spouse?.Name?.ToUpper())
                        .EndText();

                    // CMND
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(410, 948)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Đã kết hôn", customer.Spouse?.IdCard)
                        .EndText();

                    // DTDD
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(575, 948)
                        .ShowTextIf(customer.Personal?.MaritalStatus == "Đã kết hôn", customer.Spouse?.Phone)
                        .EndText();
                }
                //7.1
                {
                    var referee1 = customer.Referees.ToList()[0];
                    // Họ tên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(105, 734)
                        .ShowText(referee1.Name?.ToUpper())
                        .EndText();

                    // DT liên hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(415, 734)
                        .ShowText(referee1.Phone)
                        .EndText();

                    // Mối quan hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(620, 734)
                        .ShowText(referee1.Relationship?.ToUpper())
                        .EndText();
                }
                //7.2
                {
                    RefereeInfoPdfDto referee2 = new RefereeInfoPdfDto();
                    if (customer.Referees?.ToList().Count == 2)
                    {
                        referee2 = customer.Referees.ToList()[1];
                    }
                    // Họ tên
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(105, 696)
                        .ShowText(referee2.Name.ToUpper())
                        .EndText();

                    // DT liên hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(415, 696)
                        .ShowText(referee2.Phone)
                        .EndText();

                    // Mối quan hệ
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(620, 696)
                        .ShowText(referee2.Relationship.ToUpper())
                        .EndText();
                }
                //9.1
                {
                    // Ngân hàng
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(115, 450)
                        .ShowText($"{customer.BankInfo?.Name?.ToUpper()}")
                        .EndText();


                }
                //9.2
                {
                    // Chi nhánh
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(110, 414)
                        .ShowText($"{customer.BankInfo?.Branch?.ToUpper()}")
                        .EndText();

                    // Số tài khoản
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(472, 414)
                        .ShowText($"{customer.BankInfo?.AccountNo}")
                        .EndText();
                }

                //10.1
                {
                    // Bảo mật thông tin vay với
                    // Người thân
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(209, 313)
                        .ShowTextIf(customer.OtherInfo?.SecretWith == "Người thân", "x")
                        .EndText();

                    // Vợ/Chồng
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(323, 313)
                        .ShowTextIf(customer.OtherInfo?.SecretWith == "Vợ/Chồng", "x")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 17)
                        .MoveText(451, 313)
                        .ShowTextIf(customer.OtherInfo?.SecretWith == "Khác", "x")
                        .EndText();

                    // Khác
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(500, 313)
                        .ShowTextIf(customer.OtherInfo?.SecretWith == "Khác", customer.OtherInfo?.SecretWithOther?.ToUpper())
                        .EndText();

                    var note = SplitAddress(customer.OtherInfo?.Note, 100);
                    // Note
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 267)
                        .ShowText(note[0])
                        .EndText();
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 228)
                        .ShowText(note[1])
                        .EndText();
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 189)
                        .ShowText(note[2])
                        .EndText();
                    canvas.BeginText()
                        .SetFontAndSize(font, 13)
                        .MoveText(50, 150)
                        .ShowText(note[3])
                        .EndText();
                }

                pdfDoc.Close();

                return stream.ToArray();
            }

        }
        private string GetStringMoney(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return doc;
        }

        private string FormatMoney(string number = "")
        {
            if (number.IndexOf(",") > -1)
            {
                return number;
            }
            string result = number.Trim();
            int j = 0;
            for (int i = number.Trim().Length - 1; i > 0; i--)
            {
                if (j % 3 == 2)
                {
                    result = result.Insert(i, ",");
                }
                j++;
            }
            return result;
        }

        private string[] SplitAddress(string tempAddress, int maxSize = 60)
        {
            string tempAddressLine1 = "";
            string tempAddressLine2 = "";
            string tempAddressLine3 = "";
            string tempAddressLine4 = "";
            if (!string.IsNullOrEmpty(tempAddress))
            {
                if (tempAddress.Length > maxSize)
                {
                    var lst = tempAddress.Split(" ");
                    for (int i = 0; i < lst.Length; i++)
                    {
                        if (tempAddressLine1.Length < maxSize)
                        {
                            tempAddressLine1 += lst[i] + " ";
                        }
                        else if (tempAddressLine2.Length < maxSize)
                        {
                            tempAddressLine2 += lst[i] + " ";
                        }
                        else if (tempAddressLine3.Length < maxSize)
                        {
                            tempAddressLine3 += lst[i] + " ";
                        }
                        else
                        {
                            tempAddressLine4 += lst[i] + " ";
                        }
                    }
                }
                else
                {
                    tempAddressLine1 = tempAddress;
                }
            }
            return new string[]
                {
                    tempAddressLine1,
                    tempAddressLine2,
                    tempAddressLine3,
                    tempAddressLine4
                };
        }
    }
}
