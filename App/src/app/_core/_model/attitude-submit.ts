export interface AttitudeSubmit {
  iD: number;
  campaignID: number;
  submitFrom: number;
  submitTo: number;
  fLAttitude: boolean;
  l0Attitude: boolean;
  l1Attitude: boolean;
  l2Attitude: boolean;
  fLKPI: boolean;
  l0KPI: boolean;
  l1KPI: boolean;
  l2KPI: boolean;
  isDisplayFL: boolean;
  isDisplayL0: boolean;
  isDisplayL1: boolean;
  isDisplayL2: boolean;
  btnFL: boolean;
  btnL0: boolean;
  btnL1: boolean;
  btnL2: boolean;
  btnFLKPI: boolean;
  btnL0KPI: boolean;
  btnL1KPI: boolean;
  btnL2KPI: boolean;
  // submitTime: string | null;
}
