using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class DocumentMapping
    {
        public static readonly ReadOnlyDictionary<string, string> GREEN_E_RETURN_DOCUMENT_MAPPING =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"CMND/CCCD/PASSPORT", IdCard},
                {"SỔ HỘ KHẨU", familyBook},
                {"XÁC NHẬN CỦA KHÁCH HÀNG", customerConfirm},
                {"HƯỚNG DẪN BẢN ĐỒ", map},
                {"PHIẾU THÔNG TIN KHÁCH HÀNG", clientInfo},
                {"HỒ SƠ KHÁC", others},
            });
        private const string IdCard = @"{
                'groupId': 22,
                'groupName': 'CMND/CCCD/PASSPORT',
                'mandatory': true,
                'hasAlternate': false,
                'documents': [
                    {
                        'id': 4,
                        'documentCode': 'CivicIdentity',
                        'documentName': 'CCCD',
                        'inputDocUid': '559124662598964b868a210069922848',
                        'mapBpmVar': 'DOC_CivicIdentity',
                    },
                    {
                        'id': 6,
                        'documentCode': 'IdentityCard',
                        'documentName': 'CMND',
                        'inputDocUid': '943054199583cdc9c54f601005419993',
                        'mapBpmVar': 'DOC_IdentityCard'
                    },
                    {
                        'id': 7,
                        'documentCode': 'Passport',
                        'documentName': 'PASSPORT',
                        'inputDocUid': '263825031598964c98ee968050949384',
                        'mapBpmVar': 'DOC_Passport'
                    }
                ]
            }";
        private const string familyBook = @"{
                'groupId': 19,
                'groupName': 'Hộ khẩu',
                'mandatory': true,
                'hasAlternate': false,
                'documents': [
                    {
                        'id': 58,
                        'documentCode': 'FamilyBook',
                        'documentName': 'Sổ hộ khẩu',
                        'inputDocUid': '964050292583cdc9c56ab88092528583',
                        'mapBpmVar': 'DOC_FamilyBook'
                    }
                ]
            }";
        private const string customerConfirm = @"{
                'groupId': 26,
                'groupName': 'Xác nhận của khách hàng',
                'mandatory': true,
                'hasAlternate': false,
                'documents': [
                    {
                        'id': 23,
                        'documentCode': 'CustomerConfirm',
                        'documentName': 'Xác nhận của khách hàng',
                        'inputDocUid': '592712402583cdc9c53fc06001162945',
                        'mapBpmVar': 'DOC_CustomerConfirm'
                    }
                ]
            }";
        private const string map = @"
            {
                'groupId': 27,
                'groupName': 'Hướng dẫn bản đồ',
                'mandatory': true,
                'hasAlternate': false,
                'documents': [
                    {
                        'id': 24,
                        'documentCode': 'CustomerMap',
                        'documentName': 'Hướng dẫn bản đồ',
                        'inputDocUid': '592712402583cdc9c53fc06001162945',
                        'mapBpmVar': 'DOC_CustomerMap'
                    }
                ]
            }";
        private const string clientInfo = @"
            {
                'groupId': 34,
                'groupName': 'Phiếu thông tin Khách hàng',
                'mandatory': true,
                'hasAlternate': false,
                'documents': [
                    {
                        'id': 49,
                        'documentCode': 'CustomerInformationSheet',
                        'documentName': 'Phiếu thông tin khách hàng',
                        'inputDocUid': '6970134635989643323f1d9054171098',
                        'mapBpmVar': 'DOC_CustomerInformationSheet'
                    }
                ]
            }";
        private const string others = @"{
                'groupId': 37,
                'groupName': 'Hồ sơ khác',
                'mandatory': true,
                'hasAlternate': false,
                'documents': [
                    {
                        'id': 13,
                        'documentCode': 'ContracLabor',
                        'documentName': 'Hợp đồng lao động',
                        'inputDocUid': '425720214583cdc9c509104046115087',
                        'mapBpmVar': 'DOC_ContracLabor'
                    },
                    {
                        'id': 34,
                        'documentCode': 'PaySlip',
                        'documentName': 'Sao kê ngân hàng/bảng lương/Xác nhận lương',
                        'inputDocUid': '738875901598968883770b8015189815',
                        'mapBpmVar': 'DOC_PaySlip'
                    },
                    {
                        'id': 169,
                        'documentCode': 'Promote',
                        'documentName': 'Quyết định Bổ nhiệm hoặc Quyết định nâng bậc lương',
                        'inputDocUid': null,
                        'mapBpmVar': 'DOC_Promote'
                    },
                    {
                        'id': 125,
                        'documentCode': 'BHYT',
                        'documentName': 'BHYT',
                        'inputDocUid': null,
                        'mapBpmVar': 'BHYT'
                    },
                    {
                        'id': 70,
                        'documentCode': 'Bill',
                        'documentName': 'Giấy xác nhận cư trú/Hóa đơn điện/Hóa đơn nước/Hóa đơn internet/điện thoại có dây',
                        'inputDocUid': '967171635598968971dcdd0064707754',
                        'mapBpmVar': 'DOC_Bill'
                    },
                    {
                        'id': 43,
                        'documentCode': 'House',
                        'documentName': 'Giấy tờ sỡ hữu nhà',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_House'
                    },
                    {
                        'id': 43,
                        'documentCode': 'InsuranceContract',
                        'documentName': 'Hợp đồng bảo hiểm nhân thọ',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_InsuranceContract'
                    },
                    {
                        'id': 43,
                        'documentCode': 'BHDH',
                        'documentName': 'Bảo hiểm đáo hạn',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_BHDH'
                    },
                    {
                        'id': 43,
                        'documentCode': 'Marrage',
                        'documentName': 'Chứng minh thông tin người hôn phối',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_Marrage'
                    },
                    {
                        'id': 43,
                        'documentCode': 'Owner',
                        'documentName': 'Chứng minh sở hữu',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_Owner'
                    },

                    {
                        'id': 43,
                        'documentCode': 'Bussiness',
                        'documentName': 'Giấy phép kinh doanh',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_Bussiness'
                    },

                    {
                        'id': 43,
                        'documentCode': 'Fee',
                        'documentName': 'Biên lai đóng phí',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_Fee'
                    },
                    {
                        'id': 43,
                        'documentCode': 'Credit',
                        'documentName': 'Sao kê thẻ tín dụng',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_Credit'
                    },
                    {
                        'id': 43,
                        'documentCode': 'Other',
                        'documentName': 'Khác',
                        'inputDocUid': '508837634598971c46bc117081002262',
                        'mapBpmVar': 'DOC_Other'
                    },
                ]
            }";
    }
}