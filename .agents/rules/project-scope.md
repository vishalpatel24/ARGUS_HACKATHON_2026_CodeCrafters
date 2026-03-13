# GrantFlow Project Scope

## Core Objective
Build GrantFlow, an AI-augmented end-to-end Grant Lifecycle Management Platform servicing a single grant-making organization. The platform guides both internal staff and external applicants from initial grant discovery through final compliance reporting and fund disbursement.

## In-Scope Features

### 1. User Roles & Management
*   **External:** Applicants (NGOs, Trusts, Institutions).
*   **Internal:** Platform Admin, Program Officer, Grant Reviewer, Finance Officer.
*   **Permissions:** Strict RBAC (Role-Based Access Control) defined per the PRD.

### 2. Grant Discovery & Intake
*   **Catalogue:** Public display of 3 pre-configured grant types (CDG, EIG, ECAG).
*   **Dual-Mode Intake:** Traditional wizard-based forms AND an AI-guided conversational chatbot interface.
*   **Document Vault:** Reusable standing organizational documents.

### 3. Application Workflow Engine
*   **8 Distinct Stages:** Submitted -> Eligibility Screening -> Under Review -> Award Decision -> Agreement Sent -> Active Grant -> Reporting -> Closed.
*   **SLA Tracking:** Configured SLAs per stage with automated alerts.
*   **Audit Logging:** Immutable record of all state changes, actors, and timestamps.

### 4. Agentic AI Integration
*   **AI Boundaries:** AI serves as a *recommendation engine only*. Humans must confirm or override all final decisions. AI cannot disburse funds or communicate directly with applicants.
*   **Eligibility Screening:** Hard rule checks (e.g., funding range) and soft narrative analysis (e.g., thematic alignment).
*   **Review Package Generation:** Automated project summaries, suggested scores against rubrics, and risk flagging for human reviewers.
*   **Compliance Analysis:** Automated financial variance checks and narrative progress analysis on submitted grantee reports.

### 5. Award & Disbursement Management
*   **Agreements:** Auto-generated PDF agreements with data merging.
*   **Finance Tracking:** Configurable disbursement tranches (Inception, Mid-Project, Final, Milestone) tied to reporting approvals.

### 6. Communications
*   **In-App Messaging:** Threaded communications between Program Officers and Applicants.
*   **Notifications:** Automated, event-driven status updates and reminders (In-app and Email).

## Out-of-Scope (Not Covered in Hackathon Sprint)
*   Creation of custom, dynamically generated Grant Programme types beyond the 3 defined (CDG, EIG, ECAG).
*   Direct API integrations with external banking or payment systems for automated fund transfer (Finance tracking is manual upload/verification).
*   Complex applicant identity verification (e.g., automated MCA/Gov portal scraping). Registration relies on OTP and manual document review.
