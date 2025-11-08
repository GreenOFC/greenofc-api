import { UserStatus } from '../../../common/enums/user-status.enum';

export interface TeamMemberDto {
  id: string;
  user_name: string;
  mafc_code?: string;
  full_name?: string;
  id_card?: string;
}

export interface TeamLeadInfoDto {
  id?: string;
  user_name?: string;
  full_name?: string;
  team_members?: TeamMemberDto[];
}

export interface UserSuspensionHistoryDto {
  start_date: Date;
  end_date: Date;
  lead_source_type: string;
}

export interface UserProfileResponse {
  id: string;
  user_name: string;
  ec_dsa_code?: string;
  ec_sale_code?: string;
  mafc_code?: string;
  full_name: string;
  user_email: string;
  phone: string;
  id_card?: string;
  old_id_card?: string;
  role_name: string;
  status: UserStatus;
  role_ids: string[];
  team_lead_info?: TeamLeadInfoDto | null;
  is_active?: boolean;
  created_date?: Date | string;
  modified_date?: Date | string;
  user_suspension_histories?: UserSuspensionHistoryDto[];
  permissions: string[];
}

