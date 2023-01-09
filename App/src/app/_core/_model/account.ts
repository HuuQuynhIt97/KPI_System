import { AccountGroupAccount } from "./account-group-account";
import { AccountType } from "./account.type";

export interface Account {
  id: number;
  username: string;
  fullName: string;
  password: string;
  email: string;
  isLock: boolean;
  accountTypeId: number | null;
  factId: number | null;
  centerId: number | null;
  deptId: number | null;
  createdBy: number;
  modifiedBy: number | null;
  createdTime: string;
  modifiedTime: string | null;
  accountType: AccountType;
  accountGroupText: string;
  accountGroupIds: number[];
  leader: number;
  manager: number;
  leaderName: string;
  managerName: string;
  l1: number | null;
  l1Name: string;
  l2: number | null;
  l2Name: string;
  functionalLeader: number | null;
  functionalLeaderName: string;
  l0: boolean;
  ghr: boolean;
  jobTitleId: number | null;
  gm: boolean;
  gmScore: boolean;
  systemFlow: number | null;
}
