using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ResponseModels
{
    class EventResponseModel
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class EventDetailRespondModel : EventModel
    {
        public bool IsFollowed { get; set; }
        public List<string> EventCategories { get; set; }
        public List<int> EventCategoryIDs { get; set; }
        public int ActiveMenuItem { get; set; }
        public List<EventItemTypeModel> ItemTypes { get; set; }
        public TicketModel Ticket { get; set; }
        public RatingResponseModel Rating { get; set; }
        //public IEnumerable<SelectListItem> AvailableStores { get; set; }
    }

    public partial class EventItemTypeModel
    {
        public string DisplayName
        {
            get
            {
                if (Template == (int)DbEventItemTemplate.Speaker)
                {
                    return "Tên diễn giả";
                }
                else if (Template == (int)DbEventItemTemplate.Agenda)
                {
                    return "Nội dung";
                }
                else if (Template == (int)DbEventItemTemplate.Sponsor)
                {
                    return "Nhà tài trợ";
                }
                return "Tên";
            }
        }

        public string DisplayDescription
        {
            get
            {
                if (Template == (int)DbEventItemTemplate.Speaker)
                {
                    return "Chức vụ/Vai trò";
                }
                else if (Template == (int)DbEventItemTemplate.Agenda)
                {
                    return "Thời gian";
                }
                else if (Template == (int)DbEventItemTemplate.Sponsor)
                {
                    return "Chi tiết";
                }
                return "Mô tả";
            }
        }

        public string DisplayDetail
        {
            get
            {
                return "Chi tiết";
            }
        }
    }

}
