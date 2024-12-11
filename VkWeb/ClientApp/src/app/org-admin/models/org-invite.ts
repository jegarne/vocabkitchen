class OrgInvite {
  constructor(
    orgId: string,
    emails: string[]
  ) {
    this.orgId = orgId;
    this.emails = emails;
  }
  orgId: string;
  emails: string[];
}

export {
  OrgInvite
};
