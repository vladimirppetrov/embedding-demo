/*
 * PLEASE EXECUTE SCRIPTS ONE BY ONE. If the table is not created again from scratch, you can skip the first two scripts.
 */



--Table template_entity
ALTER TABLE template_entity ADD COLUMN priority smallint NULL;

--Inserting dummy template
INSERT INTO template_entity (
    insert_date,
    name,
    status,
    type,
    language,
	guid,
	priority
) VALUES (
    NOW(), 
    'Dummy Template',                    
    1, 
    1,  
    'EN'  ,
	'b318dc0d-eb93-484f-aab4-af82f8566a25',
	2
);

--Priority change

--Change all templates(mail/push/sms) priority to low

update template_entity 
set priority = 3
where type in (1,2,3)


--SMS templates
UPDATE template_entity
SET priority = 1
WHERE name ILIKE ANY (ARRAY[
  'verification_code.sms',
  'verification_code_android.sms',
  'tran.sms',
  'ipay_activated.sms',
  'payment_request.sms',
  'ipay_forgotten_pass.sms',
  'card_forgotten_pin.sms',
  'ipay_mpos_tran.sms',
  'payment.sms',
  'otp_code_apple_pay.sms',
  'otp_code_gpay.sms',
  'payment_request_success.sms',
  'link_payment_received.sms',
  'button_payment_received.sms',
  'payment_request_failed.sms',
  'giftcard_activated.sms',
  'giftcard_pin_request.sms',
  'ipay_active.sms',
  'ipay_act_not_found.sms',
  'ipay_act_inv_stat.sms',
  'giftcard_act_not_found.sms',
  'giftcard_change_pin_web.sms',
  'giftcard_change_pin_sms.sms',
  'ipay_act_inv_method.sms',
  'giftcard_invalid_gsm.sms',
  'giftcard_invalid_status.sms'
])
AND type = 3;

--PUSH
UPDATE template_entity
SET priority = 1
WHERE name ILIKE ANY (ARRAY[
  'auth_code.push',
  'tran_rcp.push',
  'out_trans_success.push',
  'out_trans_received.push',
  'tran.push',
  'dev_auth_request.push',
  'new_notifications.push',
  'payment_request_success.push',
  'out_trans_declined.push',
  'payment.push',
  'payment_request_failed.push',
  'apple_pay_activate.push',
  'google_activate.push',
  'card_reactivated.push'
])
AND type = 2;


UPDATE template_entity
SET priority = 3
WHERE name ILIKE ANY (ARRAY[
  'iss_campaign_first_transaction.push',
  'iss_campaign_activation_remind.push'
])
AND type = 2;

--EMAIL
UPDATE template_entity
SET priority = 1
WHERE name ILIKE ANY (ARRAY[
  'mypos_out_trans_success.html',
  'mypos_out_trans_received.html',
  'mypos_payment_request_success.html',
  'mypos_forgotten_email.html',
  'mypos_cfps_req_doc_processed.html',
  'mypos_req_new_doc_not_verif.html',
  'mypos_mpos_tran.html',
  'mypos_event_tran.html',
  'mypos_acc_statement_generated.html',
  'mypos_out_trans_declined.html',
  'mypos_successful_verification.html',
  'mypos_payment_request.html',
  'mypos_payment_request_failed.html',
  'mypos_enroll_success.html',
  'mypos_self-ident_successful.html',
  'mypos_succ_payment_with_tag.html',
  'mypos_verification_success.html',
  'mypos_out_trans_cancelled.html',
  'mypos_out_trans_beneficiary.html',
  'mypos_apple_success.html',
  'mypos_ideal_payment_success.html',
  'mypos_linked_gpay_card.html',
  'mypos_card_sub_activate.html',
  'mypos_prem_card_order.html',
  'mypos_card_sub_block.html',
  'mypos_prem_card_unsubscribe.html'
])
AND type = 1;

UPDATE template_entity
SET priority = 3
WHERE name ILIKE ANY (ARRAY[
  'mypos_1st_reminder_gpay_mpl.html',
  'mypos_2nd_reminder_gpay_mpl.html'
])
AND type = 1;
