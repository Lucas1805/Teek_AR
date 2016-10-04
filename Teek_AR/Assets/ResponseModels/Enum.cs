using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    public enum DbActivityType
    {
        [Description("Không phân loại")]
        None = 0,
        [Description("Khảo sát")]
        Survey = 1,
        [Description("Bình chọn")]
        Voting = 2,
        [Description("Hỏi đáp - Trao đổi")]
        RemoteMic = 3,
        [Description("Trò chơi")]
        Game = 4,

    }


    public enum DbSurveyQuestionType
    {
        [Description("Đơn lựa chọn")]
        SingleChoice = 1,
        [Description("Đa lựa chọn")]
        MultipleChoice = 2,
        [Description("Điền nội dung")]
        FreeText = 3,
        [Description("Đánh giá")]
        Score = 4,
        [Description("Mô tả")]
        Label = 5,
        [Description("Phân trang")]
        PageBreak = 6,
    }

    public enum DbEventItemTemplate
    {
        [Description("Diễn giả")]
        Speaker = 1,
        [Description("Chương trình")]
        Agenda = 2,
        [Description("Nhà tài trợ")]
        Sponsor = 3,
        [Description("Tùy chọn")]
        Other = 4
    }

    public enum ApproveStatus
    {
        [Description("Mới")]
        New = 0,
        [Description("Đã duyệt")]
        Approved = 1,
        [Description("Từ chối")]
        NotApproved = 2,
        [Description("Chờ duyệt")]
        Pending = 3,
    }


    public enum FilterSortEnum
    {
        [Description("Ngày tạo")]
        CreatedDate = 1,
        [Description("Địa điểm gần bạn")]
        Location = 2,
        [Description("Ngày tổ chức")]
        OrganizedDate = 3
    }

    public enum EventPriceEnum
    {
        [Description("Tất cả giá vé")]
        All = 1,
        [Description("Miễn phí")]
        Free = 2,
        [Description("Có phí")]
        Paid = 3
    }

    public enum LanguageEnum
    {
        [Description("vi")]
        Vietnamese = 0,
        [Description("en")]
        English = 1
    }

    public enum TransactionStatus
    {
        Pending = 0,
        Cancel = 1,
        Success = 2
    }

    public enum NotificationEnum
    {
        ApproveEvent = 1,
        Message = 2,
        Activity = 3,
        RemoteMic = 4,
        NewActivity = 5
    }

    public enum SendMessageEnum
    {
        ForParticipatedUser = 1,
        ForFollowedUser = 2
    }

    public enum RemoteMicStatus
    {
        Pending = 1,
        Connected = 2
    }
}
