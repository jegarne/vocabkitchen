class OrgUser {
  constructor(
    orgId: string,
    vkUserId: string
  ) {
    this.orgId = orgId;
    this.vkUserId = vkUserId;
  }
  orgId: string;
  vkUserId: string;
}

export {
  OrgUser
};
