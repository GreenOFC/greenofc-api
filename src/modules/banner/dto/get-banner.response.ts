export class GetBannerResponse {
  id: string;
  imageUrl: string;
  redirectUrl: string;
  startDate: Date;
  endDate?: Date | null;
  createdDate: Date;
}

